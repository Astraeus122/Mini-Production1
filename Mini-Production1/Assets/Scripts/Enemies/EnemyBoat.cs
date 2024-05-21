using System.Collections;
using UnityEngine;

public class EnemyBoat : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bubbles = null;

    [SerializeField]
    private ParticleSystem splashes = null;

    [SerializeField]
    private BoatMovement boat = null;

    [SerializeField]
    private Vector2 wiggleRange;

    [SerializeField]
    private BoatMovement target;

    [SerializeField]
    private float fireCommandInterval = 1f;

    private float xTarget;

    bool fireCommandStatus = false;

    private void Update()
    {
        float deltaX = target.transform.position.x - transform.position.x;

        if (Mathf.Abs(target.Movement.x) > 1f)
        {
            if (target.Movement.x < 0) xTarget = wiggleRange.y;
            else xTarget = wiggleRange.x;
        }
        else if (Mathf.Abs(deltaX) < 4)
        {
            if (target.transform.position.x < 0) xTarget = wiggleRange.y;
            else xTarget = wiggleRange.x;
        }

        SteerToTarget();

        if (Mathf.Abs(deltaX) < 3)
        {
            if (!fireCommandStatus)
            {
                StartCoroutine(RepeatFire());
                fireCommandStatus = true;
            }
        }
        else if (fireCommandStatus)
        {
            StopAllCoroutines();
            fireCommandStatus = false;
        }
    }

    private IEnumerator RepeatFire()
    {
        while(enabled)
        {
            boat.IssueCrewCommand("Fire");
            yield return new WaitForSeconds(fireCommandInterval);
            boat.IssueCrewCommand("Cease Fire");

        }
    }
    public void SetTarget(BoatMovement target)
    {
        this.target = target;
    }

    private void SteerToTarget()
    {
        float distX = xTarget - transform.position.x;

        boat.SteeringInput = Mathf.Clamp(distX, -1f, 1f);
    }

    public void StartBubbling()
    {
        bubbles.Play();
    }

    public void StopBubbling()
    {
        bubbles.Stop();
    }

    public void Splash()
    {
        splashes.Play();
    }

    public void StartControl()
    {
        boat.enabled = true;
    }

}
