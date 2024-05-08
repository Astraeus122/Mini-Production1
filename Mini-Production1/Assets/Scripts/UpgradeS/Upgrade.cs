using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public Sprite image;
    public int level = 0; // Default level at 0, indicating it's new

    public Upgrade(string name, string description, Sprite image, int level = 0)
    {
        this.name = name;
        this.description = description;
        this.image = image;
        this.level = level;
    }

    // Method to execute the upgrade
    public void ApplyUpgrade()
    {
        // Implement what happens when this upgrade is applied
        // Example: Increase health, speed, etc.
    }
}
