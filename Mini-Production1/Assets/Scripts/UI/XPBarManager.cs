using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarManager : MonoBehaviour
{
    public Image xpFillImage; // Image that fills based on XP percentage

    public TextMeshProUGUI xpText; // Displays current XP/max XP
    public TextMeshProUGUI xpToNextLevelText; // Displays total XP needed for the next level
    public TextMeshProUGUI scoreText; // Displays the score

    private float xpPercentage;
    private float maxWidth;

    private void Start()
    {
        if (xpFillImage == null || xpText == null || xpToNextLevelText == null || scoreText == null)
        {
            Debug.LogError("XPBarManager is missing references to necessary UI components.");
            return;
        }

        // Assuming the max width of the fill image is the width at 100% XP
        maxWidth = xpFillImage.rectTransform.rect.width;

        // Initialize fill image to zero width
        SetWidth(xpFillImage.rectTransform, 0);
    }

    public void UpdateXPBar(float currentXP, float xpToNextLevel, int score)
    {
        xpPercentage = currentXP / xpToNextLevel;
        float newWidth = maxWidth * xpPercentage;

        // Set the width of the fill image based on the XP percentage
        SetWidth(xpFillImage.rectTransform, newWidth);

        // Update the text elements
        xpText.text = $"{Mathf.FloorToInt(currentXP)}/{Mathf.FloorToInt(xpToNextLevel)} XP";
        xpToNextLevelText.text = $"{Mathf.FloorToInt(xpToNextLevel)} XP";
        scoreText.text = $"{score}CM";
    }

    private void SetWidth(RectTransform rectTransform, float width)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.anchoredPosition = new Vector2(width / 2, rectTransform.anchoredPosition.y); // Adjust the position to ensure it grows from the left
    }
}
