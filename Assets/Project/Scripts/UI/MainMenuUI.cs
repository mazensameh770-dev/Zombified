using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject levelsUI;

    [SerializeField] private FadeUI fadeUI;

    private void Start()
    {
        playButton.onClick.AddListener(OpenLevels);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OpenLevels()
    {
        fadeUI.FadeOut();

        Invoke(nameof(ShowLevels), fadeUI.fadeDuration);
    }

    private void ShowLevels()
    {
        gameObject.SetActive(false);

        levelsUI.SetActive(true);

        FadeUI levelsFade = levelsUI.GetComponent<FadeUI>();
        levelsFade.FadeIn();
    }

    private void OpenOptions()
    {
        fadeUI.FadeOut();

        Invoke(nameof(ShowOptions), fadeUI.fadeDuration);
    }

    private void ShowOptions()
    {
        gameObject.SetActive(false);

        optionsUI.SetActive(true);

        FadeUI optionsFade = optionsUI.GetComponent<FadeUI>();
        optionsFade.FadeIn();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        optionsButton.onClick.RemoveListener(OpenOptions);
        quitButton.onClick.RemoveListener(QuitGame);
    }
}