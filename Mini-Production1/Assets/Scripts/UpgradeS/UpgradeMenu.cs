using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject menuCanvas; 
    private BoatMovement boat;
    private CameraControl control;
    public Button healthUpgradeButton;
    public Button speedUpgradeButton;
    public Button repairSpeedUpgradeButton;
    public Button binocularsUpgradeButton;
    public Button JeremiahUpgradeButton;

    // Define TextMeshPro variables for each button
    public TMP_Text healthUpgradeText;
    public TMP_Text speedUpgradeText;
    public TMP_Text repairSpeedUpgradeText;
    public TMP_Text binocularsUpgradeText;
    public TMP_Text jeremiahUpgradeText;

    public AudioSource buttonPressAudioSource;

    // Upgrade levels and costs
    private int healthLevel = 0;
    private int speedLevel = 0;
    private int repairSpeedLevel = 0;
    public int binocularsLevel = 0; // Only 1 level
    public int jeremiahLevel = 0; // Only 1 level

    private int healthCost = 1;
    private int speedCost = 1;
    private int repairSpeedCost = 2;
    private int binocularsCost = 2; // Single upgrade cost
    private int jeremiahCost = 5; // Single upgrade cost

    private const int maxLevel = 5; // Maximum level for most upgrades

    public GameObject jeremiah;

    void Start()
    {
        UpdateButtonLabels();
        // Initialize menu as inactive
        menuCanvas.SetActive(false);

        boat = FindObjectOfType<BoatMovement>();
        if (boat == null)
        {
            Debug.LogError("BoatMovement script not found in the scene.");
        }

        // Setup button listeners
        healthUpgradeButton.onClick.AddListener(() => UpgradeHealth());
        speedUpgradeButton.onClick.AddListener(() => UpgradeSpeed());
        repairSpeedUpgradeButton.onClick.AddListener(() => UpgradeRepairSpeed());
        binocularsUpgradeButton.onClick.AddListener(() => UpgradeBinoculars());
        JeremiahUpgradeButton.onClick.AddListener(() => UpgradeJeremiah());
    }
    void UpdateButtonLabels()
    {
        // Update TextMeshPro texts
        healthUpgradeText.text = healthLevel < maxLevel ? "Health: " + healthCost + " Resources" : "Health: Max";
        speedUpgradeText.text = speedLevel < maxLevel ? "Speed: " + speedCost + " Resources" : "Speed: Max";
        repairSpeedUpgradeText.text = repairSpeedLevel < maxLevel ? "Repair Speed: " + repairSpeedCost + " Resources" : "Repair Speed: Max";
        binocularsUpgradeText.text = binocularsLevel < 1 ? "Binoculars: " + binocularsCost + " Resources" : "Binoculars: Max";
        jeremiahUpgradeText.text = jeremiahLevel < 1 ? "Jeremiah: " + jeremiahCost + " Resources" : "Jeremiah: Max";
    }

    public void ToggleMenu()
    {
        ToggleMenu(!menuCanvas.activeSelf);
    }

    public void ToggleMenu(bool active)
    {
        // Update button labels only if the menu is being activated
        if (active)
        {
            UpdateButtonLabels();
        }

        // Toggle the active state of the menu canvas
        menuCanvas.SetActive(active);
    }
    public void UpgradeHealth()
    {
        buttonPressAudioSource.Play();
        if (healthLevel < maxLevel && ProcessTransaction(healthCost))
        {
            healthLevel++;
            healthCost = CalculateNewCost(healthCost); // Update the cost
            boat.UpgradeMaxHealth(2);
        }
        UpdateButtonLabels();
    }

    public void UpgradeSpeed()
    {
        buttonPressAudioSource.Play();
        if (speedLevel < maxLevel && ProcessTransaction(speedCost))
        {
            speedLevel++;
            speedCost = CalculateNewCost(speedCost); // Update the cost
            boat.UpgradeSpeed(1);
        }
        UpdateButtonLabels();
    }
    public void UpgradeRepairSpeed()
    {
        buttonPressAudioSource.Play();
        if (repairSpeedLevel < maxLevel && ProcessTransaction(repairSpeedCost))
        {
            repairSpeedLevel++;
            repairSpeedCost = CalculateNewCost(repairSpeedCost); // Update the cost
                                                                 // Update repair duration for all leaks
            Leak[] leaks = FindObjectsOfType<Leak>();
            foreach (Leak leak in leaks)
            {
                leak.UpdateLeakRepairDuration(repairSpeedLevel);
            }
        }
        UpdateButtonLabels();
    }


    public void UpgradeBinoculars()
    {
        buttonPressAudioSource.Play();
        if (binocularsLevel < 1 && ProcessTransaction(binocularsCost))
        {
            binocularsLevel++;
        }
        UpdateButtonLabels(); // Update button labels after upgrading
    }

    public void UpgradeJeremiah()
    {
        buttonPressAudioSource.Play();
        if (jeremiahLevel < 1 && ProcessTransaction(jeremiahCost))
        {
            jeremiahLevel++;
            if (jeremiah != null)
            {
                jeremiah.SetActive(true);
            }
        }
        UpdateButtonLabels(); // Update button labels after upgrading
    }

    private int CalculateNewCost(int currentCost)
    {
        int newCost = currentCost + 2; // Increment cost by 2
        return Mathf.Min(newCost, 9); // Ensure cost does not exceed 9
    }

    private bool ProcessTransaction(int cost)
    {
        bool canAfford = GameManager.Instance.RepairResources >= cost;
        if (canAfford) GameManager.Instance.RepairResources -= cost;
        return canAfford;
    }
}
