using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject ControlCanvas;
    public GameObject MainCanvas;

    [SerializeField]
    private TMP_Text highscoreText = null;
    public AudioSource buttonPressAudioSource;

    [SerializeField, Tooltip("Use '%s' in place of the highscore value")]
    private string highscoreFormat = "Highscore: %s";

    [SerializeField]
    private Button debugResetButton = null;

    [SerializeField]
    private Button swapControlbutton;

    [SerializeField]
    private RectTransform keyControls;

    [SerializeField]
    private RectTransform gamepadControls;

    private void Awake()
    {
        debugResetButton.gameObject.SetActive(Debug.isDebugBuild);
        MainCanvas.SetActive(true); // Assuming you want the Main Canvas active at start
        ControlCanvas.SetActive(false); // Assuming you want the Control Canvas inactive at start
    }

    private void Start()
    {
        int highscore = PlayerPrefs.GetInt(GameManager.HighscorePrefKey);
        highscoreText.text = highscoreFormat.Replace("%s", highscore.ToString());
    }

    public void StartGame()
    {
        buttonPressAudioSource.Play();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ShowControls()
    {
        buttonPressAudioSource.Play();
        MainCanvas.SetActive(false); // Deactivate Main Canvas
        ControlCanvas.SetActive(true); // Activate Control Canvas
    }

    public void SwapControls()
    {
        if (keyControls.gameObject.activeInHierarchy)
        {
            keyControls.gameObject.SetActive(false);
            swapControlbutton.GetComponentInChildren<TMP_Text>().text = "View Key Controls";
            gamepadControls.gameObject.SetActive(true);
        }
        else if (gamepadControls.gameObject.activeInHierarchy)
        {
            gamepadControls.gameObject.SetActive(false);
            swapControlbutton.GetComponentInChildren<TMP_Text>().text = "View Gamepad Controls";
            keyControls.gameObject.SetActive(true);
        }
    }

    public void HideControls()
    {
        buttonPressAudioSource.Play();
        MainCanvas.SetActive(true); // Activate Main Canvas
        ControlCanvas.SetActive(false); // Deactivate Control Canvas
    }

    public void ExitGame()
    {
        buttonPressAudioSource.Play();
        Application.Quit();
    }

    public void ResetStats()
    {
        buttonPressAudioSource.Play();
        PlayerPrefs.DeleteAll();
        Start();
    }
}