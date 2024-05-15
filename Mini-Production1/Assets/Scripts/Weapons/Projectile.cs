using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hazard
{
    [SerializeField]
    private float damage = 10;

    private bool isDead = false;

    public override void OnImpacting(HazardImpactor hazardImpactor)
    {
        if (isDead) return;

        if (hazardImpactor.TryGetComponent(out BoatMovement boat))
        {
            boat.ReceiveDamage(damage, transform.position);
        }
        else if (hazardImpactor.TryGetComponent(out Obstacle_Scr obstacle))
        {
            obstacle.Die();
        }

        isDead = true;
        Destroy(gameObject);
    }
}
