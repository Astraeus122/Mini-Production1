using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.7f;

    private CameraControl cameraControl;
    private Vector3 originalLocalPosition;
    private float shakeTimeRemaining;

    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        Vector3 binocularsOffset = cameraControl.GetBinocularsOffset();

        if (shakeTimeRemaining > 0)
        {
            transform.localPosition = originalLocalPosition + binocularsOffset + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalLocalPosition + binocularsOffset;
        }
    }

    public void TriggerShake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}
