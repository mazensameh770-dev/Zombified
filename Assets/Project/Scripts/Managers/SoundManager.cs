using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonHoverClip;
    [SerializeField] private AudioClip buttonClickClip;

    [Header("Gameplay Sounds")]
    [SerializeField] private AudioClip trapPlaceClip;
    [SerializeField] private AudioClip trapRemoveClip;
    [SerializeField] private AudioClip trapClearClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip cardShuffleClip;
    [SerializeField] private AudioClip cardClickedClip;

    private const string BackgroundVolumeKey = "BackgroundVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    private void LoadVolumes()
    {
        backgroundSource.volume =
            PlayerPrefs.GetFloat(BackgroundVolumeKey, 0.25f);

        sfxSource.volume =
            PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    public void SetBackgroundVolume(float value)
    {
        backgroundSource.volume = value;

        PlayerPrefs.SetFloat(BackgroundVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;

        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float GetBackgroundVolume()
    {
        return backgroundSource.volume;
    }

    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }

    public void PlayCardShuffle()
    {
        sfxSource.PlayOneShot(cardShuffleClip);
    }

    public void PlayCardClicked()
    {
        sfxSource.PlayOneShot(cardClickedClip);
    }

    public void PlayButtonHover()
    {
        sfxSource.PlayOneShot(buttonHoverClip);
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }

    public void PlayTrapPlace()
    {
        sfxSource.PlayOneShot(trapPlaceClip);
    }
    public void PlayWin()
    {
        sfxSource.PlayOneShot(winClip);
    }

    public void PlayTrapRemove()
    {
        sfxSource.PlayOneShot(trapRemoveClip);
    }
    public void PlayTrapClear()
    {
        sfxSource.PlayOneShot(trapClearClip);
    }
}