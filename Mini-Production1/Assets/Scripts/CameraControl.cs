using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 offset;
    private Quaternion fixedRotation = Quaternion.Euler(22, 0, 0);

    public Transform boatTransform;

    void Start()
    {
        // Calculate the initial offset from the boat to the camera
        offset = transform.position - boatTransform.position;

        // Set the fixed rotation
        transform.rotation = fixedRotation;
    }

    void LateUpdate()
    {
        // Follow the boat's position without inheriting its rotation
        Vector3 targetPosition = boatTransform.position + offset;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

        // Maintain the desired rotation
        transform.rotation = fixedRotation;
    }
}
