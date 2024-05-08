using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    public float experience;

    [SerializeField]
    public float level;

    [SerializeField]
    public float experienceToNextLevel = 100.0f;

    public UpgradeManager upgradeManager;

    void Update()
    {
        experience += 0.001f;

        // Check if level up
        if (experience >= experienceToNextLevel)
        {
            level++;
            experience -= experienceToNextLevel;
            experienceToNextLevel = CalculateNextLevelExp(level);
            upgradeManager.DisplayRandomUpgrades();
        }
    }

    float CalculateNextLevelExp(float level)
    {
        // Next level experience requirement
        return (level * 100.0f) + 100.0f; 
    }
}
