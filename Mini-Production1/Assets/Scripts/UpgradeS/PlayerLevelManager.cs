using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public int currentLevel = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;  // Initial XP required to reach the next level
    public float xpIncreaseFactor = 1.5f; // Factor by which the XP requirement increases each level

    public UpgradeUI upgradeUI; // Reference to the UpgradeUI to trigger UI updates

    void Update()
    {
        AddXP(Time.deltaTime * 50); // Adding XP based on time, modify the multiplier as needed
    }

    public void AddXP(float xp)
    {
        currentXP += xp;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel; // Remove the XP needed for the previous level
        xpToNextLevel *= xpIncreaseFactor; // Increase the requirement for the next level

        // Trigger the upgrade UI
        if (upgradeUI != null)
        {
            upgradeUI.ActivateUpgradeMenu();  
        }

    }
}
