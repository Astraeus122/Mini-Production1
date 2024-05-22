using UnityEngine;

public class ObstacleActivator : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] enemies;
    public GameObject[] resources;

    [Header("Base Activation Chances")]
    public float baseObstacleChance = 0.4f;
    public float baseEnemyChance = 0.25f;
    public float baseResourceChance = 0.35f;

    [Header("Increase Rates Per 100 Points Score")]
    public float obstacleIncreasePerPoint = 0.005f / 150f;
    public float enemyIncreasePerPoint = 0.005f / 150f;
    public float resourceIncreasePerPoint = 0.005f / 150f;

    [Header("Maximum Obstacles Configuration")]
    public int initialMaxObstacles = 3;
    public float maxObstacleIncreaseRate = 0.03f; // Rate at which the max obstacles increase per point of score

    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && gameManager.IsGameActive)
        {
            float score = gameManager.Score;
            ActivateObjects(score);
        }
    }

    private void ActivateObjects(float score)
    {
        // Calculate current chances based on score
        float obstacleChance = Mathf.Clamp(baseObstacleChance + obstacleIncreasePerPoint * score, 0, 1);
        float enemyChance = Mathf.Clamp(baseEnemyChance + enemyIncreasePerPoint * score, 0, 1);
        float resourceChance = Mathf.Clamp(baseResourceChance + resourceIncreasePerPoint * score, 0, 1);

        // Calculate maximum allowed obstacles based on the score
        int maxObstacles = Mathf.Clamp(initialMaxObstacles + Mathf.FloorToInt(maxObstacleIncreaseRate * score), initialMaxObstacles, obstacles.Length);

        // Activate or deactivate each type of object based on their chances and limits
        SetActiveRandomlyWithLimit(obstacles, obstacleChance, maxObstacles);
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

    private void SetActiveRandomlyWithLimit(GameObject[] objects, float activationChance, int maxCount)
    {
        int activatedCount = 0;
        foreach (var obj in objects)
        {
            if (activatedCount >= maxCount)
            {
                obj.SetActive(false);
            }
            else
            {
                bool shouldBeActive = Random.value < activationChance;
                obj.SetActive(shouldBeActive);
                if (shouldBeActive)
                {
                    activatedCount++;
                }
            }
        }
    }
}
