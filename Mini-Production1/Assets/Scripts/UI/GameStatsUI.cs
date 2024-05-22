using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    private void Update()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            Debug.Log(gameManager.Score);
        }
    }
}
