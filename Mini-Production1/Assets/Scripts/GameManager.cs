using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Transform boat;
    public static GameManager Instance { get; private set; }

    public static string HighscorePrefKey = "PlayerHighscore";

    public bool IsGameActive { get; private set; } = true;

    public int Score { get; private set; }

    [SerializeField]
    private int startingRepairResources = 0;

    [SerializeField]
    private UpgradeUI upgradeUI;

    public int currentLevel = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;  // Initial XP required to reach the next level
    public float xpIncreaseFactor = 1.5f; // Factor by which the XP requirement increases each level

    private int backingRepairResources;
    public int RepairResources
    {
        get
        {
            return backingRepairResources;
        }
        set
        {
            backingRepairResources = value;
            OnResourceChange?.Invoke(backingRepairResources.ToString());
        }
    }

    // events purpose for UI updating hence why we convert to string
    public UnityEvent<string> OnScoreChange;
    public UnityEvent<string> OnResourceChange;

    private BoatMovement boatMovement; // Reference to the BoatMovement

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        boatMovement = boat.GetComponent<BoatMovement>();
        if (boatMovement == null)
        {
            Debug.LogError("Failed to find BoatMovement component on the boat.");
        }

        RepairResources = startingRepairResources;

        // score is set to 0 from 0 so doesnt invoke the event initially, we manually do it here
        OnScoreChange?.Invoke(Score.ToString());
    }

    private void OnDestroy()
    {
        int highscore = PlayerPrefs.GetInt(HighscorePrefKey);
        //if (highscore == default) highscore = 0; // int default IS 0

        if (Score > highscore)
            PlayerPrefs.SetInt(HighscorePrefKey, Score);
    }

    public void StopGame()
    {
        IsGameActive = false;
    }

    private void Update()
    {
        if (!IsGameActive || !IsPlayerAlive()) return;

        int oldScore = Score;
        Score = Mathf.FloorToInt(Time.timeSinceLevelLoad);

        AddXP(Time.deltaTime * 5); // Adding XP based on time, modify the multiplier as needed

        if (oldScore != Score) OnScoreChange?.Invoke(Score.ToString());
    }

    public void AddXP(float xp)
    {
        if (!IsPlayerAlive()) return;

        currentXP += xp;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        if (!IsPlayerAlive()) return;

        currentLevel++;
        currentXP -= xpToNextLevel; // Remove the XP needed for the previous level
        xpToNextLevel *= xpIncreaseFactor; // Increase the requirement for the next level

        if (upgradeUI != null)
        {
            upgradeUI.ActivateUpgradeMenu();
        }
    }

    private bool IsPlayerAlive()
    {
        return boatMovement != null && boatMovement.IsAlive;
    }
}

