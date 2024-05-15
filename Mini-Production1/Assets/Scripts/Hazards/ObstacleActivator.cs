using UnityEngine;

public class ObstacleActivator : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] enemies;
    public GameObject[] resources;

    // Difficulty thresholds based on score
    public int mediumDifficultyScore = 1000;
    public int hardDifficultyScore = 3000;

    // Activation chances for each difficulty level
    [Header("Easy Difficulty Chances")]
    public float easyObstacleChance = 0.3f;
    public float easyEnemyChance = 0.2f;
    public float easyResourceChance = 0.5f;

    [Header("Medium Difficulty Chances")]
    public float mediumObstacleChance = 0.5f;
    public float mediumEnemyChance = 0.4f;
    public float mediumResourceChance = 0.5f;

    [Header("Hard Difficulty Chances")]
    public float hardObstacleChance = 0.7f;
    public float hardEnemyChance = 0.6f;
    public float hardResourceChance = 0.4f;

    private void Start()
    {
        int score = GetCurrentPlayerScore();
        ActivateObjects(score);
    }

    private void ActivateObjects(int score)
    {
        float obstacleChance, enemyChance, resourceChance;

        // Determine difficulty based on score
        if (score >= hardDifficultyScore)
        {
            obstacleChance = hardObstacleChance;
            enemyChance = hardEnemyChance;
            resourceChance = hardResourceChance;
        }
        else if (score >= mediumDifficultyScore)
        {
            obstacleChance = mediumObstacleChance;
            enemyChance = mediumEnemyChance;
            resourceChance = mediumResourceChance;
        }
        else
        {
            obstacleChance = easyObstacleChance;
            enemyChance = easyEnemyChance;
            resourceChance = easyResourceChance;
        }

        // Activate or deactivate each type of object based on their chances
        SetActiveRandomly(obstacles, obstacleChance);
        SetActiveRandomly(enemies, enemyChance);
        SetActiveRandomly(resources, resourceChance);
    }

    private void SetActiveRandomly(GameObject[] objects, float activationChance)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(Random.value < activationChance);
        }
    }

    private int GetCurrentPlayerScore()
    {
        // Placeholder for actual score retrieval logic
        return PlayerPrefs.GetInt("PlayerScore", 0);
    }
}
