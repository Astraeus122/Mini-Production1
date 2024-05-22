using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    List<Upgrade> allUpgrades = new List<Upgrade>();

    public List<Upgrade> currentOptions = new List<Upgrade>();

    private BoatMovement boat; // BoatMovement reference

    void Awake()
    {
        boat = FindObjectOfType<BoatMovement>(); // Find and assign BoatMovement
        if (boat == null)
        {
            Debug.LogError("Failed to find BoatMovement component.");
        }
        InitializeUpgrades(); // Initialize all upgrades once the boat component is found
    }

    private void InitializeUpgrades()
    {
        allUpgrades.Add(new Upgrade("Speed Boost", "Incrementally increases the boat's maximum speed, allowing for quicker evasion and travel.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Maneuverability Enhancements", "Improves the boat's handling and turning capabilities, essential for navigating through tight spaces.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Hull Strength", "Strengthens the boat's hull to withstand more damage from collisions or enemy attacks.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Efficient Repairs", "Enhances the efficiency of repair actions, reducing the time needed to restore boat health.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Advanced Navigation Tools", "Equips the boat with advanced sensors to detect upcoming hazards and special items early.", null, boat, 1));
        allUpgrades.Add(new Upgrade("Scrap Magnet", "Installs a magnetic scrap collector that automatically attracts distant scrap pieces.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Floodlights", "Upgrades the boat's lighting system to enhance visibility in dark environments, illuminating more area.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Shield Generator", "Installs a protective shield that temporarily absorbs any damage from one hit, recharging over time.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Impact Bumpers", "Outfits the boat with reinforced bumpers that reduce the damage taken from frontal collisions.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Cannon Upgrades", "Improves the boat's cannon range, reload speed, and damage, enabling more effective defense and offense.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Temporal Shift", "Activates a device that temporarily slows everything except the player, offering tactical maneuver advantages.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Regeneration Module", "Installs a regeneration module that continuously restores boat health over time.", null, boat, int.MaxValue));
        allUpgrades.Add(new Upgrade("Jeremiah", "Jeremiah will repair your boat, he is cool.", null, boat, 2));
        allUpgrades.Add(new Upgrade("Waterproof Seal", "Nothing some Flextape can't fix!", null, boat, int.MaxValue));
    }

    public void GenerateUpgradeOptions()
    {
        currentOptions.Clear();
        List<Upgrade> availableUpgrades = allUpgrades.Where(u => u.level <= u.maxLevel).ToList();
        if (availableUpgrades.Count >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, availableUpgrades.Count);
                currentOptions.Add(availableUpgrades[index]);
                availableUpgrades.RemoveAt(index);
            }
        }
        else
        {
            Debug.LogError("Not enough upgrades to display. Need at least 3.");
        }
    }
}


