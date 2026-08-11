using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("VSync")]
    [SerializeField] private Button vsyncButton;
    [SerializeField] private Image vsyncButtonImage;
    [SerializeField] private Sprite vsyncOnSprite;
    [SerializeField] private Sprite vsyncOffSprite;

    [Header("Audio")]
    [SerializeField] private Slider backgroundSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private const string VSyncKey = "VSyncEnabled";
    private const string BackgroundVolumeKey = "BackgroundVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private bool vsyncEnabled;

    private void Start()
    {
        // VSync
        vsyncEnabled = PlayerPrefs.GetInt(VSyncKey, 1) == 1;

        ApplyVSync();
        UpdateVSyncUI();

        vsyncButton.onClick.AddListener(ToggleVSync);

        // Audio
        LoadAudioSettings();

        backgroundSlider.onValueChanged.AddListener(SetBackgroundVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Navigation
        backButton.onClick.AddListener(GoBack);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        fadeUI.FadeIn();
    }

    private void GoBack()
    {
        fadeUI.FadeOut();

        Invoke(nameof(ShowMainMenu), 0.2f);
    }

    private void ShowMainMenu()
    {
        gameObject.SetActive(false);
        mainMenuUI.SetActive(true);

        FadeUI mainMenuFade = mainMenuUI.GetComponent<FadeUI>();
        mainMenuFade.FadeIn();
    }

    private void ToggleVSync()
    {
        vsyncEnabled = !vsyncEnabled;

        PlayerPrefs.SetInt(VSyncKey, vsyncEnabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyVSync();
        UpdateVSyncUI();
    }

    private void ApplyVSync()
    {
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
    }

    private void UpdateVSyncUI()
    {
        vsyncButtonImage.sprite = vsyncEnabled ? vsyncOnSprite : vsyncOffSprite;
    }

    private void LoadAudioSettings()
    {
        float backgroundVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        backgroundSlider.value = backgroundVolume;
        sfxSlider.value = sfxVolume;
    }

    private void SetBackgroundVolume(float value)
    {
        PlayerPrefs.SetFloat(BackgroundVolumeKey, value);
        PlayerPrefs.Save();
    }

    private void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float GetBackgroundVolume()
    {
        return PlayerPrefs.GetFloat(BackgroundVolumeKey, 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    private void OnDestroy()
    {
        vsyncButton.onClick.RemoveListener(ToggleVSync);
        backgroundSlider.onValueChanged.RemoveListener(SetBackgroundVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        backButton.onClick.RemoveListener(GoBack);
    }
}