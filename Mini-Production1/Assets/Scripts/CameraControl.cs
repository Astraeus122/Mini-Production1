using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform target; // The target object to follow
    public Vector3 initialOffsetPosition = new Vector3(0, 30, -5); // Initial position offset relative to the target
    public float initialRotationX = 80.0f; // Initial fixed X rotation angle for a top-down view

    private Vector3 targetOffsetPosition = new Vector3(0, 7, -7); // Target offset position for transition
    private float targetRotationX = 40.0f; // Target X rotation for transition
    private float transitionTime = 2.0f; // Duration of the transition in seconds
    private bool isAtTargetPosition = false; // State flag to track the camera's position state

    private Vector3 currentOffsetPosition; // Holds the current dynamic offset position
    private float currentRotationX; // Holds the current dynamic X rotation angle

    void Start()
    {
        // Initialize current position and rotation to initial values
        currentOffsetPosition = initialOffsetPosition;
        currentRotationX = initialRotationX;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isAtTargetPosition)
            {
                StartCoroutine(TransitionCamera(targetOffsetPosition, targetRotationX));
            }
            else
            {
                StartCoroutine(TransitionCamera(initialOffsetPosition, initialRotationX));
            }
            isAtTargetPosition = !isAtTargetPosition; // Toggle the state
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Update the position to follow the target with the current dynamic offset
        transform.position = target.position + currentOffsetPosition;

        // Match the rotation of the target, but override the X rotation to maintain the current dynamic view
        transform.rotation = Quaternion.Euler(currentRotationX, target.eulerAngles.y, target.eulerAngles.z);
    }

    IEnumerator TransitionCamera(Vector3 newOffsetPosition, float newRotationX)
    {
        float elapsedTime = 0;
        Vector3 startingOffset = currentOffsetPosition;
        float startingRotationX = currentRotationX;

        while (elapsedTime < transitionTime)
        {
            float fraction = elapsedTime / transitionTime;
            currentOffsetPosition = Vector3.Lerp(startingOffset, newOffsetPosition, fraction);
            currentRotationX = Mathf.Lerp(startingRotationX, newRotationX, fraction);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentOffsetPosition = newOffsetPosition;
        currentRotationX = newRotationX;
    }
}