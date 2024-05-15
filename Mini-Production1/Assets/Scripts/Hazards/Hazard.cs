using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    [field: SerializeField]
    public int TeamId { get; private set; } = 0;

    public abstract void OnImpacting(HazardImpactor hazardImpactor);
}
