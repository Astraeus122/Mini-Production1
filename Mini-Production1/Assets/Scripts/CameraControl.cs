using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform boatTransform; // Reference to the boat's transform
    private Vector3 offsetPosition = new Vector3(0, 20, -9); // Offset position relative to the boat
    private Quaternion fixedRotation = Quaternion.Euler(40, 0, 0); // Fixed rotation for the camera
    private float transitionTime = 2.0f;
    private bool isAtTargetPosition = false;

    void Start()
    {
        // Set the initial position and rotation
        transform.position = boatTransform.position + offsetPosition;
        transform.rotation = fixedRotation;
    }

    void Update()
    {
        // Update the camera position to follow the boat with offset
        transform.position = boatTransform.position + offsetPosition;

        // Check for transition trigger
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(TransitionCamera(
                isAtTargetPosition ? boatTransform.position + offsetPosition * 2 : boatTransform.position + offsetPosition,
                isAtTargetPosition ? fixedRotation * Quaternion.Euler(40, 0, 0) : fixedRotation));

            isAtTargetPosition = !isAtTargetPosition;
        }
    }

    IEnumerator TransitionCamera(Vector3 newPosition, Quaternion newRotation)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < transitionTime)
        {
            float fraction = elapsedTime / transitionTime;
            transform.position = Vector3.Lerp(startingPosition, newPosition, fraction);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, fraction);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Explicitly set the final position and rotation
        transform.position = newPosition;
        transform.rotation = newRotation;
    }
}
