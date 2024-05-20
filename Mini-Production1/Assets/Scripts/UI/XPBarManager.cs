using UnityEngine;
using UnityEngine.UI;

public class XPBarManager : MonoBehaviour
{
    public Image xpFillImage;
    public Image xpGlowImage;
    public RectTransform xpBarBackground;

    private float xpPercentage;
    private float maxWidth;

    private void Start()
    {
        if (xpFillImage == null || xpGlowImage == null || xpBarBackground == null)
        {
            Debug.LogError("XPBarController is missing references to necessary UI components.");
            return;
        }

        // Assuming the max width of the bar background is the width at 100% XP
        maxWidth = xpBarBackground.rect.width;

        // Initialize fill and glow images to zero width
        SetWidth(xpFillImage.rectTransform, 0);
        SetWidth(xpGlowImage.rectTransform, 0);
    }

    public void UpdateXPBar(float currentXP, float xpToNextLevel)
    {
        xpPercentage = currentXP / xpToNextLevel;
        float newWidth = maxWidth * xpPercentage;

        // Set the width of the fill and glow images based on the XP percentage
        SetWidth(xpFillImage.rectTransform, newWidth);
        SetWidth(xpGlowImage.rectTransform, newWidth);

        // Adjust the glow color intensity if needed
        xpGlowImage.color = Color.Lerp(Color.clear, Color.white, xpPercentage);
    }

    private void SetWidth(RectTransform rectTransform, float width)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
