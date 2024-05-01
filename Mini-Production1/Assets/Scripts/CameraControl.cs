using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera gameCamera;
    public float forwardOffset = 60.0f; // Distance to move forward
    private UpgradeMenu menu;

    private Vector3 binocularsOffset = Vector3.zero;
    private bool isCameraMoved = false;

    void Start()
    {
        if (gameCamera == null)
        {
            gameCamera = Camera.main;
        }

        menu = FindObjectOfType<UpgradeMenu>();
    }

    void Update()
    {
        if (menu.binocularsLevel == 1)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isCameraMoved)
            {
                binocularsOffset = new Vector3(0, 0, forwardOffset);
                isCameraMoved = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q) && isCameraMoved)
            {
                binocularsOffset = Vector3.zero;
                isCameraMoved = false;
            }
        }

        gameCamera.transform.localPosition = binocularsOffset;
    }

    public Vector3 GetBinocularsOffset()
    {
        return binocularsOffset;
    }
}

