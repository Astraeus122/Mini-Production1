using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController), typeof(Interactor))]
public class CrewmateDriver : MonoBehaviour
{
    private enum CrewmateState
    {
        Standby,
        Tracking,
        Repairing,
        Reset
    }


    [SerializeField]
    private float trackingTimeout = 5f;

    [SerializeField]
    private BoatMovement boatAboard = null;

    [SerializeField]
    private Transform standbyLocation = null;

    [SerializeField]
    private bool teleportToStandbyOnStart = true;

    private CrewmateState state;
    private Leak targettedLeak;
    private float resetTimer;

    private CharacterController controller;
    private Interactor interactor;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        interactor = GetComponent<Interactor>();

        state = CrewmateState.Standby;
    }

    private void Start()
    {
        if (teleportToStandbyOnStart)
            transform.position = standbyLocation.position;
    }

    private void OnEnable()
    {
        interactor.enabled = true;
    }

    private void OnDisable()
    {
        interactor.enabled = false;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case CrewmateState.Standby:
                {
                    MoveTowards(standbyLocation.position, 0.2f);

                    SearchForLeak();
                }
                break;
            case CrewmateState.Tracking:
                {
                    resetTimer += Time.fixedDeltaTime;

                    if (resetTimer >= trackingTimeout)
                    {
                        // time to walk to leak site has run out, go back home and try again
                        targettedLeak = null;
                        state = CrewmateState.Reset;
                        break;
                    }

                    if (!targettedLeak || !targettedLeak.enabled)
                    {
                        // leak has since been repaired or removed
                        targettedLeak = null;
                        state = CrewmateState.Standby;
                        break;
                    }

                    if (targettedLeak && interactor.Focus && targettedLeak.gameObject == interactor.Focus.gameObject)
                    {
                        // have arrived at leak site
                        controller.MoveInput = Vector3.zero;
                        state = CrewmateState.Repairing;
                        interactor.TryStartInteract();
                        break;
                    }
                    // on way to leak site still with no issues
                    MoveTowards(targettedLeak.transform.position, 0f);
                }
                break;
            case CrewmateState.Repairing:
                {
                    if (!targettedLeak.enabled)
                    {
                        // done repairing
                        interactor.StopInteract();
                        state = CrewmateState.Standby;
                        break;
                    }
                    else
                    {
                        // we have repairs to do
                        // ensure we are still repairing... if out of range correct by going back to tracking
                        if (!interactor.Focus || interactor.Focus.gameObject != targettedLeak.gameObject)
                        {
                            interactor.StopInteract();
                            resetTimer = 0;
                            state = CrewmateState.Tracking;
                            break;
                        }
                    }
                }
                break;
            case CrewmateState.Reset:
                {
                    // this state is a safeguard for straight-line pathfinding. if he is stuck tracking for x
                    // seconds he resets back to standby which hopefully has a straight path to most of the leaks

                    if (MoveTowards(standbyLocation.position, 0.2f))
                    {
                        state = CrewmateState.Standby;
                    }
                }
                break;
        }
    }

    private void SearchForLeak()
    {
        (Leak obj, float sqrDist) closest = (null, Mathf.Infinity);

        foreach (var leak in boatAboard.leakSites)
        {
            if (!leak.enabled) continue;

            float sqrDist = (interactor.Origin.position - leak.transform.position).sqrMagnitude;

            if (sqrDist > closest.sqrDist) continue;
            
            closest.sqrDist = sqrDist;
            closest.obj = leak;
            
        }

        if (closest.obj)
        {
            targettedLeak = closest.obj;
            resetTimer = 0;
            state = CrewmateState.Tracking;
        }
    }

    private bool MoveTowards(Vector3 point, float stoppingRange)
    {
        Vector3 delta = point - transform.position;
        Vector2 delta2D = new Vector2(delta.x, delta.z);
        if (delta2D.sqrMagnitude > stoppingRange * stoppingRange)
        {
            controller.MoveInput = delta2D;
            return false;
        }

        controller.MoveInput = Vector2.zero;
        return true;
    }
}
