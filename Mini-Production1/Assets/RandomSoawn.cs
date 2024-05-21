using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSoawn : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    float spawnChance;
    void Awake()
    {
        var ran = Random.Range(0f, 1f);
        if (ran > spawnChance)
        {
            gameObject.SetActive(false);
        }
    }
}
