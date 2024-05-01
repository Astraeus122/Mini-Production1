using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField, Tooltip("The maximum speed the controller can walk at [units/s].")]
    private float maxWalkSpeed = 1f;

    [SerializeField, Min(0.01f), Tooltip("The controller accelerates up to it's max speed, this controls the rate of acceleration [units/s^2].")]
    private float acceleration = 1f;

    [SerializeField, Min(0.01f), Tooltip("The controller decelerates to a stop when no input is supplied, this controls the rate of deceleration [units/s^2].")]
    private float deceleration = 1f;

    [Header("Look")]

    [SerializeField]
    private float lookSpeed = 5f;

    [SerializeField, Tooltip("The child object that actually rotates to face mouse.")]
    private Transform rotatedBody = null;

    [Header("Mounting")]

    [SerializeField, Tooltip("Models the vertical deviation from the linear path the player takes when mounting a mount.")]
    AnimationCurve mountAnimJump = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Velocity Inheritance")]

    // since in prototype we dont do a ground check we can just hard code one ground  object (since in this case its always going to be the boat)
    [SerializeField]
    private MovingPlatform platform;

    private Rigidbody rb;
    private Vector2 currentWalkInput = Vector2.zero;

    Vector3 dismountOffset;
    Coroutine mountingCoroutine;

    public Vector2 MoveInput
    {
        get
        {
            return currentWalkInput;
        }
        set
        {
            currentWalkInput = value.normalized;
        }
    }
    public Mount CurrentMount { get; private set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CurrentMount) return;

        UpdateLookDirection();
    }

    private void FixedUpdate()
    {
        if (CurrentMount) return;

        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        bool intendingMotion = currentWalkInput.sqrMagnitude > 0.01f;

        Vector3 input = (currentWalkInput.x * transform.right + currentWalkInput.y * transform.forward);
        rb.AddForce((intendingMotion ? acceleration : deceleration) * (input - rb.velocity / maxWalkSpeed) * rb.mass);

    }

    private void UpdateLookDirection()
    {
        Vector3 lookDir = rb.velocity;
        lookDir.y = 0;

        if (lookDir.sqrMagnitude < 0.001f) return;

        rotatedBody.rotation = Quaternion.Slerp(rotatedBody.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * lookSpeed);
    }

    public void Mount(Mount mount)
    {
        if (CurrentMount != null) return;

        if (mountingCoroutine != null) return;

        mountingCoroutine = StartCoroutine(MoveToMount(mount));

        // temp fix (stop interaction while mounted)
        GetComponent<Interactor>().enabled = false;
    }

    public bool TryDismount()
    {
        if (CurrentMount == null) return false;

        if (mountingCoroutine != null) return false;

        mountingCoroutine = StartCoroutine(MoveFromMount());

        // temp fix
        GetComponent<Interactor>().enabled = true;

        return true;
    }


    private IEnumerator MoveToMount(Mount mount)
    {
        InputInstance.Controls.Character.Movement.Disable();

        // disable physic
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.velocity = Vector3.zero;

        // save dismount position
        dismountOffset = transform.position - mount.MountPoint.position;

        // lerp animation
        Vector3 initialPos = transform.position;
        Quaternion initialRotation = rotatedBody.rotation;
        
        if (mount.MountTime > 0)
        {
            float t = 0;
            while (t < 1)
            {
                transform.position = Vector3.Lerp(initialPos, mount.MountPoint.position, t);
                rotatedBody.rotation = Quaternion.Slerp(initialRotation, mount.MountPoint.rotation, t);
                transform.Translate(Vector3.up * mountAnimJump.Evaluate(t));

                t += Time.deltaTime / mount.MountTime;

                yield return new WaitForEndOfFrame();
            }
        }

        // ensure we stay on mount if its moving a bit
        transform.SetParent(mount.MountPoint);

        // also just incase of overshoot of the lerp, zero the position too
        transform.localPosition = Vector3.zero;

        // establish 2-way reference with mount
        CurrentMount = mount;
        mount.Passenger = this;

        mountingCoroutine = null;

        // dont re-enable movement control
    }

    private IEnumerator MoveFromMount()
    {
        InputInstance.Controls.Character.Movement.Disable(); // ensure not moving (should already be disabled anyway)

        // do the animation
        if (CurrentMount.MountTime > 0)
        {
            float t = 0;
            while (t < 1)
            {
                transform.position = Vector3.Lerp(CurrentMount.MountPoint.position, CurrentMount.MountPoint.position + dismountOffset, t);
                transform.Translate(Vector3.up * mountAnimJump.Evaluate(t));

                t += Time.deltaTime / CurrentMount.MountTime;

                yield return new WaitForEndOfFrame();
            }
        }

        // unparent from mount
        transform.SetParent(platform ? platform.transform : null);

        // incase of lerp overshoot, zero position on the dismount position
        transform.position = CurrentMount.MountPoint.position + dismountOffset;

        // re-enable physic
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.velocity = Vector3.zero;

        //disestablish 2-way reference with mount
        CurrentMount.Passenger = null;
        CurrentMount = null;

        mountingCoroutine = null;

        InputInstance.Controls.Character.Movement.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out MovingPlatform mp)) return;

        platform = mp;
        transform.SetParent(mp.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out MovingPlatform mp)) return;

        if (mp == platform)
        {
            platform = null;
            transform.SetParent(null);
        }
    }
}
