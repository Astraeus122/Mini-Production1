using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarGradient : MonoBehaviour
{
    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private Image image;


    private void Update()
    {
        image.color = gradient.Evaluate(image.fillAmount);
    }
}
