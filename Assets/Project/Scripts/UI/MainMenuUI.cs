using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button howToPlayButton;

    [Header("UI")]
    [SerializeField] private OptionsUI optionsUI;
    [SerializeField] private HowToPlayUI howToPlayUI;
    [SerializeField] private GameObject levelsUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private void Awake()
    {
        if (howToPlayButton == null)
        {
            Transform htp = transform.Find("HowToPlay");
            if (htp != null) howToPlayButton = htp.GetComponentInChildren<Button>(true);
        }

        if (howToPlayUI == null)
        {
            howToPlayUI = transform.root.GetComponentInChildren<HowToPlayUI>(true);
        }
    }

    private void Start()
    {
        playButton.onClick.AddListener(OpenLevels);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        if (howToPlayButton != null) howToPlayButton.onClick.AddListener(OpenHowToPlay);
    }
    private void OpenLevels()
    {
        fadeUI.Hide();
        levelsUI.SetActive(true);
    }

    private void OpenOptions()
    {
        fadeUI.Hide();
        optionsUI.Open(gameObject);
    }

    private void OpenHowToPlay()
    {
        fadeUI.Hide();
        if (howToPlayUI != null) howToPlayUI.Open(gameObject);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OpenLevels);
        optionsButton.onClick.RemoveListener(OpenOptions);
        quitButton.onClick.RemoveListener(QuitGame);
        if (howToPlayButton != null) howToPlayButton.onClick.RemoveListener(OpenHowToPlay);
    }
}
