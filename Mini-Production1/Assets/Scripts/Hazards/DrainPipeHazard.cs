using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainPipeHazard : Hazard
{
    [SerializeField]
    private float damagePerSecond = 0.15f;

    public override void OnImpacting(HazardImpactor hazardImpactor)
    {
        if (hazardImpactor.TryGetComponent(out BoatMovement boat))
        {
            boat.TakeDamage(damagePerSecond * Time.deltaTime, true);
        }
    }
}
