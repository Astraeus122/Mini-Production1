using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform translationTarget;

    [SerializeField]
    private Transform rotationTarget;

    [SerializeField]
    private float followSpeed = 12f;

    private Vector3 offset;


    void Awake()
    {
        // Calculate the initial offset from the boat to the camera
        offset = transform.position - translationTarget.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, translationTarget.position + offset, Time.deltaTime * followSpeed);

        transform.LookAt(rotationTarget ? rotationTarget : translationTarget);
    }
}
