using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    // Static dictionary to keep levels persistent
    private static Dictionary<string, int> levels = new Dictionary<string, int>();

    private BoatMovement boat;
    public string name;
    public string description;
    public Sprite image;
    public int level = 0;
    private int repairSpeedLevel = 0;
    public AudioSource buttonPressAudioSource;

    // Updated constructor to accept BoatMovement
    public Upgrade(string name, string description, Sprite image, BoatMovement boat)
    {
        this.name = name;
        this.description = description;
        this.image = image;
        this.boat = boat;

        // Initialize level from static dictionary
        if (levels.ContainsKey(name))
            level = levels[name];
        else
            levels[name] = level;
    }

    public void ApplyUpgrade()
    {
        this.level++;  // Always increment level first
        levels[name] = level;  // Update static dictionary with new level

        switch (name)
        {
            case "Speed Boost":
                boat.Speed += 5;  // Example: Increase speed by 5 units
                break;
            case "Maneuverability Enhancements":
                boat.yawAmount += 0.2f;  // Increase the yaw amount to improve maneuverability
                boat.yawSpeed += 0.1f;  // Optionally, increase yaw speed
                break;
            case "Hull Strength":
                boat.UpgradeMaxHealth(20);  // Use existing method to upgrade max health
                break;
            case "Efficient Repairs":
                boat.leakSusceptibility -= 0.05f;  // Decrease susceptibility to leaks
                break;
            case "Advanced Navigation Tools":
                boat.ActivateAdvancedNavigation();
                break;
            case "Scrap Magnet":
                boat.IncreaseScrapMagnetRadius(5);  // Example radius increment
                break;
            case "Floodlights":
                boat.EnhanceFloodlights(10);  // Example range increment
                break;
            case "Shield Generator":
                boat.ActivateShield(100);  // Example shield strength
                break;
            case "Impact Bumpers":
                //boat.CollisionResistance += 0.1f;  // Reduce collision damage
                break;
            case "Cannon Upgrades":
                // This could affect properties related to offensive capabilities, such as weapon power or firing rate
                break;
            case "Temporal Shift":
                // Add functionality in BoatMovement to slow down time or similar effects
                break;
            case "Hyper Drive":
                // Implement a boost in speed that ignores collisions temporarily
                break;
            case "Regeneration Module":
                // Implement a passive health regeneration mechanism in BoatMovement
                break;
            default:
                Debug.LogWarning($"Unknown upgrade name: {name}");
                break;
        }
    }
}
