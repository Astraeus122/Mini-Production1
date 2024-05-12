using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class SpawnVolume : MonoBehaviour
{
    [SerializeField]
    private Color debugColor = Color.white;

    [SerializeField]
    private BoxCollider volume;


    private void Awake()
    {
        if (!volume && TryGetComponent(out volume)) ; // intentianal empty as out param does what i need already
    }
    public Vector3 GetPoint()
    {
        return new Vector3(
            Random.Range(volume.bounds.min.x, volume.bounds.max.x),
            Random.Range(volume.bounds.min.y, volume.bounds.max.y),
            Random.Range(volume.bounds.min.z, volume.bounds.max.z)
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;

        Gizmos.DrawWireCube(volume.bounds.center, volume.bounds.size);
    }
}
