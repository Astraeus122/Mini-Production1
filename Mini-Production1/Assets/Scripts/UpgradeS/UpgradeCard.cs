using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
    private UpgradeOption currentUpgrade;

    public void Setup(UpgradeOption upgrade)
    {
        currentUpgrade = upgrade;
        // Setup other UI elements as necessary
    }
        
    public void OnClick()
    {
        currentUpgrade.ApplyUpgrade();
        // Optionally, close the upgrade UI or handle other logic
    }
}
