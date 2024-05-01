using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketStation : MonoBehaviour
{
    [SerializeField]
    private BoatMovement boat = null;

    [SerializeField, Tooltip("Health gain per second.")]
    private float bailRate = 0.4f;

    private void Update()
    {
        Debug.Log("Chilly");
        boat.TakeDamage(-bailRate * Time.deltaTime);
    }
}
