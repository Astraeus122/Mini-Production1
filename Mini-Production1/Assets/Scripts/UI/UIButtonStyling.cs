using UnityEngine;
using UnityEngine.UI;

public class UIButtonStyling : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        ColorBlock colors = myButton.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = Color.cyan;
        colors.pressedColor = Color.blue;
        colors.selectedColor = Color.green;
        myButton.colors = colors;
    }
}
