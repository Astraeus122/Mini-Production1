using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(GameManager.Instance.Score);
    }
}
