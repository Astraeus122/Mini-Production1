using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.SceneManagement;

public class DeathScreenHandler : MonoBehaviour
{
    public GameObject audioObject;
    public GameObject deathScreenCanvas;
    public TMP_Text deathMessageText;
    public AudioSource buttonPressAudioSource;

    private string[] deathMessages = new string[]
    {
        "Shipwrecked! Even the best captains have their off days.",
        "Looks like it's a sinking feeling for our brave rat captain!",
        "Abandon ship! Time to jump onto a floating cheese piece?",
        "Alas, even the mightiest rat sailors meet their watery match.",
        "Oops! Hit an obstacle, not the cheese. Try again?",
        "Your journey ends here, brave captain. Ready for another voyage?",
        "The sea claims another. Will you challenge the waves again?",
        "Lost at sea. The ocean is unforgiving, but perseverance prevails.",
        "The boat has sunk, but the spirit of adventure lives on. Set sail again?",
        "Overwhelmed by the tides. The quest for mastery continues."
    };

    private bool isDeathScreenActive = false;

    public void ShowDeathScreen()
    {
        if (!isDeathScreenActive)
        {
            Time.timeScale = 0.0f;
            // Access the AudioSource component
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.Stop();

            // Select a random message
            string randomMessage = deathMessages[Random.Range(0, deathMessages.Length)];
            deathMessageText.text = randomMessage;

            // Activate the death screen
            deathScreenCanvas.SetActive(true);
            isDeathScreenActive = true;
        }
    }

    public void RetryGame()
    {
        Time.timeScale = 1.0f;
        buttonPressAudioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        deathScreenCanvas.SetActive(false);
        isDeathScreenActive = false;
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1.0f;
        buttonPressAudioSource.Play();
        SceneManager.LoadScene("StartMenu");
        deathScreenCanvas.SetActive(false);
        isDeathScreenActive = false;
    }

    public void ExitGame()
    {
        buttonPressAudioSource.Play();
        // Exit the game
        Application.Quit();
    }
}
