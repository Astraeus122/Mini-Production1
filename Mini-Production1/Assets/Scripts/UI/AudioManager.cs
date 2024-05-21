using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip resourceCollectionSound;
    public AudioClip temporalShiftActivateSound;
    public AudioClip temporalShiftDeactivateSound;
    public AudioClip tapeSound;
    public AudioClip catapultFireSound;
    public AudioClip turretFireSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayResourceCollection()
    {
        audioSource.PlayOneShot(resourceCollectionSound);
    }

    public void PlayTemporalShiftActivate()
    {
        PlaySound(temporalShiftActivateSound, 0.6f);
    }

    public void PlayTemporalShiftDeactivate()
    {
        PlaySound(temporalShiftDeactivateSound, 0.6f);
    }
    public void PlayTapeSound()
    {
        PlaySound(tapeSound, 0.5f);
    }
    public void PlayCatapultFire()
    {
        PlaySound(catapultFireSound);
    }

    public void PlayTurretFire()
    {
        PlaySound(turretFireSound, 0.1f);
    }
    private void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        float originalVolume = audioSource.volume;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
        audioSource.volume = originalVolume;
    }
}
