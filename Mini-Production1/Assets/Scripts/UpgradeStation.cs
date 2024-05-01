using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    private UpgradeMenu upgradeMenu;

    void Awake()
    {
        upgradeMenu = FindObjectOfType<UpgradeMenu>(); // Find the UpgradeMenu script in the scene
    }

    private void OnEnable()
    {
        upgradeMenu.ToggleMenu(true);
    }
    private void OnDisable()
    {
        upgradeMenu.ToggleMenu(false);
    }
}
