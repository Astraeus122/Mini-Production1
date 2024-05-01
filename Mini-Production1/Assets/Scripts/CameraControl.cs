using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private Vector3 initialPosition = new Vector3(0, 40, 0);
    private Vector3 targetPosition = new Vector3(0, 20, -9);
    private Quaternion initialRotation = Quaternion.Euler(80, 0, 0);
    private Quaternion targetRotation = Quaternion.Euler(40, 0, 0);
    private float transitionTime = 2.0f;
    private bool isAtTargetPosition = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(TransitionCamera(
                isAtTargetPosition ? initialPosition : targetPosition,
                isAtTargetPosition ? initialRotation : targetRotation));

            isAtTargetPosition = !isAtTargetPosition;
        }
    }

    IEnumerator TransitionCamera(Vector3 newPosition, Quaternion newRotation)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.localPosition;
        Quaternion startingRotation = transform.localRotation;

        while (elapsedTime < transitionTime)
        {
            float fraction = elapsedTime / transitionTime;
            transform.localPosition = Vector3.Lerp(startingPosition, newPosition, fraction);
            transform.localRotation = Quaternion.Lerp(startingRotation, newRotation, fraction);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Explicitly set the final position and rotation
        transform.localPosition = newPosition;
        transform.localRotation = newRotation;
    }
}