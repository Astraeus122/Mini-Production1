using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoatMovement : MonoBehaviour
{
    public float speed = 3.5f;
    public float rotationSpeed = 10.0f;
    private Vector3 currentVelocity;
    private bool isAnchored = false;
    private float currentRotationVelocity;
    public float inertiaDuration = 2.0f; // Time in seconds to stop completely from max speed
    public float tiltAngle = 8.0f; // Maximum tilt angle

    public float maxHitPoints = 5;
    public Leak[] leakSites;
    public float leakSusceptibility = 0.3f;
    public float yawAmount = 2.0f; // The maximum yaw angle
    public float yawSpeed = 2.0f; // How quickly the boat yaws
    private bool isInDrainLiquid = false;
    public float drainDamagePerSecond = 1f;
    public Image healthFillUI;
    public Transform mountPoint;
    public AudioSource HitAudioSource;

    public ScreenShake screenShake;
    private float currentHitPoints;
    private Quaternion originalRotation;

    public GameObject despawnVFX;

    [SerializeField] [Tooltip("Clamp position of ship (min value)")] [Range(0, -6f)]
    private float minX;

    [SerializeField] [Range(0, 6f)] [Tooltip("Clamp position of ship (max value)")]
    private float maxX;

    public bool IsControlEnabled { get; set; } = false;

    public string DangerTag = "";
    public string CrateTag = "";
    public string DrainTag = "";

    public UnityEvent OnBoatDied;


    [SerializeField] private Transform boatWaterFill = null;
    [SerializeField] private Vector2 boatWaterMinMaxY = new Vector2(0, 1);

    public void UpgradeMaxHealth(float additionalHealth)
    {
        maxHitPoints += additionalHealth;
        currentHitPoints = maxHitPoints; // reset current health to max
    }

    public void UpgradeSpeed(float additionalSpeed)
    {
        speed += additionalSpeed;
    }

    void Start()
    {
        screenShake = Camera.main.GetComponent<ScreenShake>();
        currentHitPoints = maxHitPoints;
        originalRotation = transform.rotation; // Save the original rotation
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (IsControlEnabled && !isAnchored)  // Check if the boat is not anchored
        {
            if (input.x != 0)
            {
                RotateBoat(input.x);
            }

            if (input.y != 0)
            {
                ApplyThrust(input.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleAnchor();
        }

        if (isInDrainLiquid)
        {
            TakeDamage(drainDamagePerSecond * Time.deltaTime);
        }

        ProcessLeaks();

        ApplyInertia(input);
        MoveBoat();
        TiltBoat();
    }

    private void ToggleAnchor()
    {
        isAnchored = !isAnchored;
        if (isAnchored)
        {
            StartCoroutine(AnchorEffect());
        }
        else
        {
            // Reset the boat's state when the anchor is lifted
            currentVelocity = Vector3.zero;
            currentRotationVelocity = 0;
        }
    }
    IEnumerator AnchorEffect()
    {
        float timeToStop = 5.0f; // Increase this duration to make the stop more gradual
        float initialRotationEffect = 0.2f;
        float timer = 0;

        // Determine the initial direction based on the current rotational velocity
        int rotationDirection = currentRotationVelocity >= 0 ? 1 : -1;

        while (timer < timeToStop && isAnchored)  // Ensure this only runs if still anchored
        {
            timer += Time.deltaTime;
            float progress = timer / timeToStop;

            // Apply a smoother deceleration curve, such as using the square of progress
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, progress * progress);
            // Apply the rotation effect based on the initial direction, decreasing it over time
            currentRotationVelocity += rotationDirection * initialRotationEffect * (1 - Mathf.Sqrt(progress));

            yield return null;
        }

        if (isAnchored)  // Finalize only if still anchored
        {
            currentVelocity = Vector3.zero;
            currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, 0, Time.deltaTime * 5.0f);
        }
    }


    private void RotateBoat(float input)
    {
        // Base rotation amount calculated from input and rotation speed
        float rotationAmount = input * rotationSpeed * Time.deltaTime;

        // Adjust rotation amount by 0.02 in the direction of input
        if (input > 0)
        {  // Turning right
            rotationAmount += 0.02f;
        }
        else if (input < 0)
        {  // Turning left
            rotationAmount -= 0.02f;
        }

        // Apply the calculated rotation velocity
        currentRotationVelocity += rotationAmount;
    }

    private void ApplyThrust(float input)
    {
        Vector3 thrust = transform.forward * input * speed * Time.deltaTime;
        currentVelocity += thrust;
    }

    private void ApplyInertia(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, inertiaDuration * Time.deltaTime);
        }
        // Update the inertia effect on rotation to be dependent on the rotation speed
        currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, 0, inertiaDuration / rotationSpeed * Time.deltaTime);
    }

    private void MoveBoat()
    {
        transform.position += currentVelocity * Time.deltaTime;
        transform.Rotate(0, currentRotationVelocity * Time.deltaTime, 0);
    }

    private void TiltBoat()
    {
        // Adjust tilt based on rotation velocity for a dynamic visual effect
        float tilt = -currentRotationVelocity * tiltAngle / rotationSpeed;
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, tilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

public void TakeDamage(float damage, Vector3 impactPoint, float maxDistFromImpact = Mathf.Infinity,
        float releakStrengthThreshold = 0.5f)
    {
        if (!TakeDamage(damage)) return;

        // Trigger screen shake
        if (screenShake != null)
        {
            screenShake.TriggerShake();
        }

        // explosion on damage taken
        var explosion = Instantiate(despawnVFX);
        explosion.transform.position = gameObject.transform.position;

        HitAudioSource.Play();
        // spring leak that is closest to the impact site and less than cutoff distance away
        float sqrDistClosest = Mathf.Infinity;
        Leak closest = null;

        foreach (var leakSite in leakSites)
        {
            if (leakSite.enabled && leakSite.LeakStrength > releakStrengthThreshold) continue;

            Vector3 delta = leakSite.transform.position - impactPoint;
            if (delta.sqrMagnitude < sqrDistClosest && delta.sqrMagnitude < maxDistFromImpact * maxDistFromImpact)
            {
                closest = leakSite;
                sqrDistClosest = delta.sqrMagnitude;
            }
        }

        if (closest)
        {
            closest.enabled = false;
            closest.enabled = true;
        }
    }

    public bool TakeDamage(float damage)
    {
        if (damage == 0) return false;
        if (damage > 0 && currentHitPoints <= 0) return false;
        if (damage < 0 && currentHitPoints >= maxHitPoints) return false;

        // it is take damage, but supports both direcitons / can be used to heal
        currentHitPoints = Mathf.Clamp(currentHitPoints - damage, 0, maxHitPoints);

        float fillAmount = 1 - currentHitPoints / maxHitPoints;

        if (healthFillUI)
            healthFillUI.fillAmount = Mathf.Clamp01(fillAmount);

        if (boatWaterFill)
        {
            boatWaterFill.gameObject.SetActive(fillAmount > 0);
            boatWaterFill.localPosition = new Vector3(boatWaterFill.localPosition.x,
                Mathf.Lerp(boatWaterMinMaxY.x, boatWaterMinMaxY.y, fillAmount), boatWaterFill.localPosition.z);
        }


        if (currentHitPoints <= 0)
        {
            speed = 0;

            // sink boat
            StartCoroutine(SinkBoatRoutine());

            GameManager.Instance.StopGame();
            OnBoatDied?.Invoke();
        }

        return true;
    }

    private IEnumerator SinkBoatRoutine()
    {
        while (true) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.003f, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }

    private void ProcessLeaks()
    {
        foreach (var leak in leakSites)
        {
            TakeDamage(leak.LeakStrength * leakSusceptibility * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DangerTag))
        {
            if (other.TryGetComponent(out Obstacle_Scr obstacle))
            {
                if (obstacle.Die())
                    TakeDamage(other.GetComponent<Obstacle_Scr>().Value,
                        other.transform
                            .position); //TODO add damage field to the obstacle instance, different ones could do different dmg
            }
        }

        if (other.CompareTag(CrateTag))
        {
            GameManager.Instance.RepairResources += (int)other.GetComponent<Obstacle_Scr>().Value;
            other.gameObject.GetComponent<Obstacle_Scr>().Die();
        }

        if (other.CompareTag(DrainTag))
        {
            // Trigger screen shake
            if (screenShake != null)
            {
                screenShake.TriggerShake();
            }

            isInDrainLiquid = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(DrainTag))
        {
            isInDrainLiquid = false;
        }
    }
}