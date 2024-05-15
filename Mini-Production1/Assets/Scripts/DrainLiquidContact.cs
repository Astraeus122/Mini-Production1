using UnityEngine;

public class DrainLiquidContact : Hazard
{
    [SerializeField]
    private float healthDrainRate = 0.1f; // The rate at which health is drained per second


    public override void OnImpacting(HazardImpactor hazardImpactor)
    {
        if (hazardImpactor.TryGetComponent(out BoatMovement boat))
            boat.TakeDamage(healthDrainRate * Time.deltaTime);
    }
}
