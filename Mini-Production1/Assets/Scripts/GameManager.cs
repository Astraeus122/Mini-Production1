using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public XPBarManager xpBarManager; // Reference to the XPBarManager

    public Transform boat;

    public static string HighscorePrefKey = "PlayerHighscore";

    public bool IsGameActive { get; private set; } = true;

    public float Score { get; private set; } // Use float to track precise distance

    [SerializeField]
    private int startingRepairResources = 0;

    [SerializeField]
    public UpgradeUI upgradeUI;

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

    // Events for UI updating
    public UnityEvent<string> OnScoreChange;
    public UnityEvent<string> OnResourceChange;

    private BoatMovement boatMovement; // Reference to the BoatMovement

    private void Start()
    {
        InitializeGameManager();
    }

    public void InitializeGameManager()
    {
        boatMovement = boat.GetComponent<BoatMovement>();
        if (boatMovement == null)
        {
            Debug.LogError("Failed to find BoatMovement component on the boat.");
        }

        RepairResources = startingRepairResources;

        // Score is set to 0 from 0 so it doesn't invoke the event initially, we manually do it here
        OnScoreChange?.Invoke(Score.ToString());

        // Check for XPBarManager reference
        if (xpBarManager == null)
        {
            Debug.LogError("XPBarManager reference is missing in GameManager.");
        }
    }

    private void OnDestroy()
    {
        int highscore = PlayerPrefs.GetInt(HighscorePrefKey);

        if (Score > highscore)
            PlayerPrefs.SetInt(HighscorePrefKey, (int)Score); // Cast Score to int for highscore
    }

    public void StopGame()
    {
        IsGameActive = false;
    }

    private void Update()
    {
        if (!IsGameActive || !IsPlayerAlive()) return;

        float oldScore = Score;
        Score = Time.timeSinceLevelLoad * 10; // Assuming the rat runs 10 cm per second

        AddXP(Time.deltaTime * 5); // Adding XP based on time, modify the multiplier as needed

        if (Mathf.FloorToInt(oldScore) != Mathf.FloorToInt(Score))
        {
            OnScoreChange?.Invoke(Score.ToString());
            xpBarManager.UpdateXPBar(currentXP, xpToNextLevel, Mathf.FloorToInt(Score)); // Update the XP bar with the score
        }
    }

    public void AddXP(float xp)
    {
        if (!IsPlayerAlive()) return;

        currentXP += xp;

        // Update the XP bar and text elements
        if (xpBarManager != null)
        {
            xpBarManager.UpdateXPBar(currentXP, xpToNextLevel, Mathf.FloorToInt(Score));
        }

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        if (!IsPlayerAlive()) return;

        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= xpIncreaseFactor;

        if (xpBarManager != null)
        {
            xpBarManager.UpdateXPBar(currentXP, xpToNextLevel, Mathf.FloorToInt(Score)); // Update the XP bar for the new level
        }

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
