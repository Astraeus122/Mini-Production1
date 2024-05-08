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
            
            transform.parent.RotateAround(transform.position, new Vector3(0,1,0), -45);
        }
    }
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}