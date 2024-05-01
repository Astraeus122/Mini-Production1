using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mainCameraTransform = Camera.main.transform;
        transform.LookAt(
            transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up
            );
    }
}
