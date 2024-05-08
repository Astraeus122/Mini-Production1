using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradeTemplate;
    public UpgradeManager upgradeManager;

    void OnEnable()
    {
        ActivateUpgradeMenu();  // Call the new method here to avoid duplication
    }

    public void ActivateUpgradeMenu()
    {
        if (upgradeManager != null)
        {
            upgradeManager.GenerateUpgradeOptions();
            DisplayUpgrades();
        }
    }

    void DisplayUpgrades()
    {
        // Clear existing upgrade options to avoid duplication on UI
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Upgrade upgrade in upgradeManager.currentOptions)
        {
            GameObject upgradeOption = Instantiate(upgradeTemplate, transform);
            upgradeOption.GetComponentInChildren<Text>().text = $"{upgrade.name}\n{upgrade.description}\nLevel: {upgrade.level}";
            upgradeOption.transform.Find("UpgradeImage").GetComponent<Image>().sprite = upgrade.image;
            upgradeOption.GetComponent<Button>().onClick.AddListener(() => ApplyUpgrade(upgrade));
        }
    }

    void ApplyUpgrade(Upgrade upgrade)
    {
        upgrade.ApplyUpgrade();
        gameObject.SetActive(false); // Optionally hide the UI after an upgrade is applied
    }
}
