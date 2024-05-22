using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpawner : MonoBehaviour
{
    [SerializeField]
    private BoatMovement player;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private float shipThreshold = 100;

    [SerializeField]
    private EnemyBoat[] enemyShipPrefabs = null;

    [SerializeField]
    private SpawnVolume shipSpawnVolume = null;


    EnemyBoat activeShip = null;

    float shipNeed;


    private void Update()
    {
        if (!activeShip)
            shipNeed += Time.deltaTime;

        if (shipNeed > shipThreshold)
        {
            (activeShip = Instantiate(
                enemyShipPrefabs[UnityEngine.Random.Range(0, enemyShipPrefabs.Length)],
                shipSpawnVolume.GetPoint(),
                Quaternion.identity)
                ).Init(player, gameManager);

            shipNeed = 0f;
        }
    }
}
