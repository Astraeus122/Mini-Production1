using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera gameCamera;
    public Transform boat; // Reference to the boat's transform
    public float transitionSpeed = 5f; // Speed of the camera transition
    public float elevationAngle = 30f; // Angle of camera elevation in degrees
    public float followDistance = 10f; // Distance behind the boat

    private Vector3 targetPosition;
    private bool inTransition = false; // Flag to manage transition state

    void Start()
    {
        if (gameCamera == null)
        {
            gameCamera = Camera.main;
        }
        // Initial camera position set to top down directly above the boat
        gameCamera.transform.position = boat.position + Vector3.up * 20;
        gameCamera.transform.LookAt(boat.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            inTransition = !inTransition;
        }

        if (inTransition)
        {
            // Calculate the position behind and above the boat
            Vector3 followVector = -boat.forward * followDistance + Vector3.up * (followDistance * Mathf.Tan(elevationAngle * Mathf.Deg2Rad));
            targetPosition = boat.position + followVector;
            // Transition to the new position smoothly
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            gameCamera.transform.LookAt(boat.position + boat.forward * 10); // Adjusts the look at point slightly ahead of the boat for a more natural perspective
        }
        else
        {
            // Top-down view
            targetPosition = boat.position + Vector3.up * 20;
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            gameCamera.transform.LookAt(boat.position);
        }
    }
}
