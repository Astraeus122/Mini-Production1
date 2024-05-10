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

    private float xTarget;


    private void Update()
    {
        float x = target.transform.position.x - transform.position.x;

        if (Mathf.Abs(target.Movement.x) > 1f)
        {
            if (target.Movement.x < 0) xTarget = wiggleRange.y;
            else xTarget = wiggleRange.x;
        }
        else if (Mathf.Abs(x) < 4)
        {
            if (target.transform.position.x < 0) xTarget = wiggleRange.y;
            else xTarget = wiggleRange.x;
        }

        SteerToTarget();
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
