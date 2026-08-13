using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [Header("UI")]
    [SerializeField] private OptionsUI optionsUI;
    [SerializeField] private GameObject levelsUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private void Start()
    {
        playButton.onClick.AddListener(OpenLevels);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
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

    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OpenLevels);
        optionsButton.onClick.RemoveListener(OpenOptions);
        quitButton.onClick.RemoveListener(QuitGame);
    }
}
