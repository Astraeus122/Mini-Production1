using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Transform boat;
    public static GameManager Instance { get; private set; }

    public static string HighscorePrefKey = "PlayerHighscore";

    public bool IsGameActive { get; private set; } = true;

    public int Score { get; private set; }

    [SerializeField]
    private int startingRepairResources = 0;


    private int backingRepairResources;
    public int RepairResources
    {
        get
        {
            return backingRepairResources;
        }
        set
        {
            backingRepairResources = value;
            OnResourceChange?.Invoke(backingRepairResources.ToString());
        }
    }

    // events purpose for UI updating hence why we convert to string
    public UnityEvent<string> OnScoreChange;
    public UnityEvent<string> OnResourceChange;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        RepairResources = startingRepairResources;

        // score is set to 0 from 0 so doesnt invoke the event initially, we manually do it here
        OnScoreChange?.Invoke(Score.ToString());

    }

    private void OnDestroy()
    {
        int highscore = PlayerPrefs.GetInt(HighscorePrefKey);
        //if (highscore == default) highscore = 0; // int default IS 0

        if (Score > highscore)
            PlayerPrefs.SetInt(HighscorePrefKey, Score);
    }

    public void StopGame()
    {
        IsGameActive = false;
    }

    private void Update()
    {
        if (!IsGameActive) return;

        int oldScore = Score;
        Score = Mathf.FloorToInt(Time.timeSinceLevelLoad);

        if (oldScore != Score) OnScoreChange?.Invoke(Score.ToString());
    }

}
