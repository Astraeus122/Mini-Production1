using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    private static Dictionary<string, int> levels = new Dictionary<string, int>();

    private BoatMovement boat;
    public string name;
    public string description;
    public Sprite image;
    public int level = 0;
    public int maxLevel;
    public AudioSource buttonPressAudioSource;

    public Upgrade(string name, string description, Sprite image, BoatMovement boat, int maxLevel)
    {
        this.name = name;
        this.description = description;
        this.image = image;
        this.boat = boat;
        this.maxLevel = maxLevel;

        if (levels.ContainsKey(name))
            level = levels[name];
        else
            levels[name] = level;
    }

    public void ApplyUpgrade()
    {
        if (level >= maxLevel)
        {
            Debug.LogWarning($"{name} has reached its maximum level.");
            return;
        }

        level++;
        levels[name] = level;  // Ensure the level is saved in the static dictionary

        switch (name)
        {
            case "Speed Boost":
                boat.Speed += 5;
                break;
            case "Maneuverability Enhancements":
                boat.yawAmount += 0.2f;
                boat.yawSpeed += 0.1f;
                break;
            case "Hull Strength":
                boat.UpgradeMaxHealth(2);
                break;
            case "Efficient Repairs":
                foreach (var leak in boat.LeakSites)
                {
                    leak.UpdateLeakRepairDuration();
                }
                break;
            case "Waterproof Seal":
                boat.leakSusceptibility -= 0.03f;
                boat.leakSusceptibility = Mathf.Max(0, boat.leakSusceptibility);

                // Play the tape sound
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayTapeSound();
                }
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
                boat.maxShieldHits++;
                boat.ActivateShield();
                break;
            case "Impact Bumpers":
                boat.collisionDamageReduction += 0.1f;
                break;
            case "Cannon Upgrades":
                GameObject cannonObject = GameObject.FindWithTag("Turret");
                Shoot shootScript = cannonObject?.GetComponent<Shoot>();
                if (shootScript != null)
                {
                    shootScript.UpgradeCannon();
                }
                break;
            case "Temporal Shift":
                boat.temporalShiftUpgradeLevel++;
                break;
            case "Regeneration Module":
                boat.healthRegenerationRate += 0.03f;
                break;
            case "Jeremiah":
                boat.AddJeremiah();
                break;
            default:
                Debug.LogWarning($"Unknown upgrade name: {name}");
                break;
        }
    }
}
