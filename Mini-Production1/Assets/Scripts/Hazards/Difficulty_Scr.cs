using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty_Scr : MonoBehaviour
{
    [SerializeField]
    private Obst_Spawner_Scr[] Spawners;

    public int DifficultyLevel = 0;
    
    [Range(0.0f,10.0f)]
    [Tooltip("How much to increase by")]
    public float SpeedIncriment = 0.5f;

    [Range(0.0f, 10.0f)]
    [Tooltip("How much to reduction by")]
    public float SpawnTimeReduction = 0.5f;

    [SerializeField]
    [Tooltip("how long until increase in difficulty")]
    private float DifficultyInterval = 30.0f;
    [SerializeField]
    private float TimeCurrent = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Spawners = GameObject.FindObjectsOfType<Obst_Spawner_Scr>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Spawners != null)
        {
            if (TimeCurrent >= DifficultyInterval)
            {
                foreach (Obst_Spawner_Scr spawner in Spawners)
                {
                    if (spawner != null)
                    {
                        spawner.UpdateSpeed(spawner.Movement_Speed + SpeedIncriment);
                        spawner.SpawnRate = spawner.SpawnRate - SpawnTimeReduction;
                        DifficultyLevel += 1;
                    }
                }
                TimeCurrent = 0.0f;
            }
            TimeCurrent += Time.deltaTime;
        }
    }
}
