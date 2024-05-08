using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeOption> allUpgrades;
    public GameObject upgradeCardPrefab;
    public Transform cardsParent;

    // Function to pick 3 random upgrades
    public void DisplayRandomUpgrades()
    {
        List<UpgradeOption> selectedUpgrades = new List<UpgradeOption>();

        for (int i = 0; i < 3; i++)
        {
            int randIndex = Random.Range(0, allUpgrades.Count);
            selectedUpgrades.Add(allUpgrades[randIndex]);
        }

        foreach (UpgradeOption upgrade in selectedUpgrades)
        {
            GameObject card = Instantiate(upgradeCardPrefab, cardsParent);
            SetupCard(card, upgrade);
        }
    }

    void SetupCard(GameObject card, UpgradeOption upgrade)
    {
        card.GetComponentInChildren<Text>().text = upgrade.name;
        card.GetComponentInChildren<Image>().sprite = upgrade.image;
        card.GetComponentInChildren<UpgradeCard>().Setup(upgrade);
    }
}
