using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingArrow : MonoBehaviour
{
    public float bobbingSpeed = 2f;
    public float bobbingAmount = 0.5f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = new Vector3(originalPosition.x, originalPosition.y + newY, originalPosition.z);
    }
}
