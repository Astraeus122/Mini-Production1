using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Collider entryRange = null;

    private void OnValidate()
    {
        if (entryRange) entryRange.isTrigger = true;
    }
}
