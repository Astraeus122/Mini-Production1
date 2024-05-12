using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SewerTurn : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            Debug.Log("SewerTurn Triggered:");
            triggered = true;
            //sewer.transform.localRotation = Quaternion.Euler(0,-90+rotationWalk,0f);
            var boat = GameManager.Instance.boat;
            //boat.GetComponent<BoatMovement>().originalRotation = Quaternion.Euler(boat.localRotation.x, boat.localRotation.y+45f, boat.localRotation.z);
            
            //transform.parent.RotateAround(transform.position, new Vector3(0,1,0), -45);
            StartCoroutine(RotateAroundTarget(-45, 2f));
        }
    }

    private IEnumerator RotateAroundTarget(float angle, float duration)
    {
        Vector3 axis = Vector3.up;
        float elapsed = 0f;
        float currentAngle = 0f;

        while (elapsed<duration)
        {
            float stepAngle = Mathf.Lerp(0, angle, elapsed / duration) - currentAngle;
            currentAngle += stepAngle;
            transform.parent.parent.RotateAround(transform.position, axis, stepAngle);

            elapsed += Time.deltaTime;
            yield return null;
        }
        //transform.parent.parent.RotateAround(transform.position, axis, angle - currentAngle);
    }
    
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
    
    float EaseInOut(float t)
    {
        // Using the cubic ease-in-out curve: t^3 / (t^3 + (1 - t)^3)
        return t * t * t / (t * t * t + (1 - t) * (1 - t) * (1 - t));
    }
}
