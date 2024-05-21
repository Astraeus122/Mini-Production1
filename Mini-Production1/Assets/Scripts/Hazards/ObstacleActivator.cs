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

    [Header("Increase Rates Per Point Score")]
    public float obstacleIncreasePerPoint = 0.0005f;
    public float enemyIncreasePerPoint = 0.0005f;
    public float resourceIncreasePerPoint = 0.0005f;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            int score = GameManager.Instance.Score;
            ActivateObjects(score);
        }
    }

    private void ActivateObjects(int score)
    {
        // Calculate current chances based on score
        float obstacleChance = Mathf.Clamp(baseObstacleChance + obstacleIncreasePerPoint * score, 0, 1);
        float enemyChance = Mathf.Clamp(baseEnemyChance + enemyIncreasePerPoint * score, 0, 1);
        float resourceChance = Mathf.Clamp(baseResourceChance + resourceIncreasePerPoint * score, 0, 1);

        // Calculate maximum allowed obstacles
        int maxObstacles = Mathf.CeilToInt(obstacles.Length * Mathf.Clamp(obstacleChance + 0.2f, 0, 1));

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
