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
    [SerializeField] private GameObject parentUI; 

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private const string VSyncKey = "VSyncEnabled";

    private bool vsyncEnabled;

    private void Start()
    {
        vsyncEnabled = PlayerPrefs.GetInt(VSyncKey, 1) == 1;

        ApplyVSync();
        UpdateVSyncUI();

        vsyncButton.onClick.AddListener(ToggleVSync);

        backgroundSlider.value = SoundManager.Instance.GetBackgroundVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        backgroundSlider.onValueChanged.AddListener(SetBackgroundVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        backButton.onClick.AddListener(GoBack);
    }
    public void Open(GameObject caller)
    {
        parentUI = caller;
        gameObject.SetActive(true);
    }

    private void GoBack()
    {
        fadeUI.Hide();

        if (parentUI != null) parentUI.SetActive(true);
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
        vsyncButtonImage.sprite =
            vsyncEnabled ? vsyncOnSprite : vsyncOffSprite;
    }

    private void SetBackgroundVolume(float value)
    {
        SoundManager.Instance.SetBackgroundVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    private void OnDestroy()
    {
        vsyncButton.onClick.RemoveListener(ToggleVSync);
        backgroundSlider.onValueChanged.RemoveListener(SetBackgroundVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        backButton.onClick.RemoveListener(GoBack);
    }
}