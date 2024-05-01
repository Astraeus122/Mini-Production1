using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainLiquidContact : MonoBehaviour
{
    public float healthDrainRate = 0.1f; // The rate at which health is drained per second
    private bool isInContact = false;

    private void Update()
    {
        if (isInContact)
        {
            GetComponent<BoatMovement>().TakeDamage(healthDrainRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "VFX_Drain_liquid_1")
        {
            isInContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "VFX_Drain_liquid_1")
        {
            isInContact = false;
        }
    }
}
