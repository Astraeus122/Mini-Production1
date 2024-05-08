using UnityEngine;

[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public Sprite image;
    public int level;

    public UpgradeOption(string name, string description, Sprite image, int level)
    {
        this.name = name;
        this.description = description;
        this.image = image;
        this.level = level;
    }

    // Add a method to apply the upgrade effect
    public void ApplyUpgrade()
    {
        // Implementation depends on the type of upgrade
        // Example: Increase health, increase speed, etc.
    }
}
