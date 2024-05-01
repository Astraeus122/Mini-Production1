using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.7f;

    private Vector3 originalLocalPosition;
    private float shakeTimeRemaining;
    private Vector3 lastShakeOffset = Vector3.zero;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = transform.localPosition - lastShakeOffset + shakeOffset;
            lastShakeOffset = shakeOffset;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = transform.localPosition - lastShakeOffset;
            lastShakeOffset = Vector3.zero;  // Reset shake offset after the shaking stops
        }
    }

    public void TriggerShake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}
