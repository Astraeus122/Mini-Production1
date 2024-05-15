using UnityEngine;

public class HazardImpactor : MonoBehaviour
{
    [SerializeField]
    private int teamId = 0;

    /*private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hazard hazard)) return;

        print(hazard.TeamId + " " + teamId);
        if (hazard.TeamId != teamId)
            hazard.OnImpacting(this);
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Hazard hazard)) return;

        if (hazard.TeamId != teamId)
            hazard.OnImpacting(this);
    }
}
