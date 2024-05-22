using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradeTemplate;
    public UpgradeManager upgradeManager;

    // GameObjects for each upgrade option
    public GameObject upgrade1, upgrade2, upgrade3;

    // Text components for each upgrade card
    [SerializeField]
    TextMeshProUGUI titleText1, descriptionText1, levelText1;
    [SerializeField]
    Image upgradeImage1;

    [SerializeField]
    TextMeshProUGUI titleText2, descriptionText2, levelText2;
    [SerializeField]
    Image upgradeImage2;

    [SerializeField]
    TextMeshProUGUI titleText3, descriptionText3, levelText3;
    [SerializeField]
    Image upgradeImage3;
    private Upgrade currentUpgrade;

    void OnEnable()
    {
        ActivateUpgradeMenu();
    }

    public void ActivateUpgradeMenu()
    {
        gameObject.SetActive(true); // Ensure the UI is active
        Time.timeScale = 0; // Pause the game

        if (upgradeManager != null)
        {
            upgradeManager.GenerateUpgradeOptions();
            DisplayUpgrades();
        }
    }

    void DisplayUpgrades()
    {
        if (upgradeManager == null || upgradeManager.currentOptions == null || upgradeManager.currentOptions.Count < 3)
        {
            Debug.LogError("Not enough upgrades generated or UpgradeManager is null.");
            return;
        }

        // Update each upgrade card manually using direct references
        UpdateUpgradeCard(titleText1, descriptionText1, levelText1, upgradeImage1, upgradeManager.currentOptions[0]);
        UpdateUpgradeCard(titleText2, descriptionText2, levelText2, upgradeImage2, upgradeManager.currentOptions[1]);
        UpdateUpgradeCard(titleText3, descriptionText3, levelText3, upgradeImage3, upgradeManager.currentOptions[2]);
    }

    void UpdateUpgradeCard(TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, TextMeshProUGUI levelText, Image upgradeImage, Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogError("Upgrade is null.");
            return;
        }

        titleText.text = upgrade.name;
        descriptionText.text = upgrade.description;
        levelText.text = "Level: " + upgrade.level;
        upgradeImage.sprite = upgrade.image;
    }

    public void SetCurrentUpgrade(Upgrade upgrade)
    {
        currentUpgrade = upgrade;
    }

    public void SelectAndApplyUpgrade(int index)
    {
        if (upgradeManager == null || upgradeManager.currentOptions == null || index >= upgradeManager.currentOptions.Count)
        {
            Debug.LogError("Index out of range or UpgradeManager is null.");
            return;
        }

        Upgrade selectedUpgrade = upgradeManager.currentOptions[index];
        if (selectedUpgrade != null)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.PlayButtonClick();
            }
            selectedUpgrade.ApplyUpgrade(); // Apply the upgrade
            UpdateUpgradeDisplay(); // Refresh the display of upgrades
            CloseUpgradeMenu();
        }
        else
        {
            Debug.LogError("Selected upgrade is null.");
        }
    }

    void UpdateUpgradeDisplay()
    {
        DisplayUpgrades();  // Re-display the upgrades with updated levels
    }

    public void CloseUpgradeMenu()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.PlayButtonClick();
        }
        gameObject.SetActive(false); // Hide the UI
        Time.timeScale = 1; // Resume the game
    }
}
