using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> allUpgrades;
    public List<Upgrade> currentOptions = new List<Upgrade>();

    public Sprite speedImage;
    public Sprite healthSprite;

    void Start()
    {
        InitializeUpgrades();
    }

    void InitializeUpgrades()
    {
        // Initialize your upgrades here
        // Example:
        allUpgrades.Add(new Upgrade("Speed Boost", "Increases player speed by 20%", speedImage));
        allUpgrades.Add(new Upgrade("Health Increase", "Increases player health by 10 points", healthSprite));
        // Add other upgrades similarly
    }

    public void GenerateUpgradeOptions()
    {
        currentOptions.Clear();
        for (int i = 0; i < 3; i++) // Assuming you show 3 upgrades at a time
        {
            int index = Random.Range(0, allUpgrades.Count);
            currentOptions.Add(allUpgrades[index]);
            allUpgrades.RemoveAt(index); // Optional: Remove to avoid repeat in the same draw
        }
    }
}
