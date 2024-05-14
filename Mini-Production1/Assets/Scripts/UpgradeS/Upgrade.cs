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

        switch (name)
        {
            case "Speed Boost":
                boat.Speed += 5;  
                break;
            case "Maneuverability Enhancements":
                boat.yawAmount += 0.2f;  // Increase the yaw amount to improve maneuverability
                boat.yawSpeed += 0.1f;  // Optionally, increase yaw speed
                break;
            case "Hull Strength":
                boat.UpgradeMaxHealth(2);  // Use existing method to upgrade max health
                break;
            case "Efficient Repairs":
                foreach (var leak in boat.leakSites)
                {
                    leak.UpdateLeakRepairDuration(); // Assuming you add a method to update the duration
                }
                break;
            case "Waterproof Seal":
                boat.leakSusceptibility -= 0.03f; // Decrease susceptibility to leaks
                boat.leakSusceptibility = Mathf.Max(0, boat.leakSusceptibility); // Ensure it doesn't go negative
                break;
            case "Advanced Navigation Tools":
                boat.ActivateAdvancedNavigation();  
                break;
            case "Scrap Magnet":
                boat.IncreaseScrapMagnetRadius(10.0f, 5.1f);
                break;
            case "Floodlights":
                boat.EnhanceFloodlights(2.5f);
                break;
            case "Shield Generator":
                boat.maxShieldHits++;  // Increase the maximum hits the shield can take
                boat.ActivateShield();  // Reactivate or refresh the shield
                break;
            case "Impact Bumpers":
                boat.collisionDamageReduction += 0.1f;  // Increment damage reduction by 10%
                break;
            case "Cannon Upgrades":
                GameObject cannonObject = GameObject.FindWithTag("Turret"); // Ensure your cannon GameObject has this tag
                Shoot shootScript = cannonObject?.GetComponent<Shoot>();
                if (shootScript != null)
                {
                    shootScript.UpgradeCannon();
                }
                break;
            case "Temporal Shift":
                boat.temporalShiftUpgradeLevel++;  // Increment the upgrade level
                break;
            case "Hyper Drive":
                // Implement a boost in speed that ignores collisions temporarily
                break;
            case "Regeneration Module":
                boat.healthRegenerationRate += 0.03f;  // Increase regeneration rate by 0.05 per second per level
                break;
            case "Jeremiah":
                boat.AddJeremiah();  // Spawn another Jeremiah instance
                break;
            default:
                Debug.LogWarning($"Unknown upgrade name: {name}");
                break;
        }
    }
}
