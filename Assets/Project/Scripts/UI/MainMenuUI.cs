using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

<<<<<<< Updated upstream
=======
    [Header("UI")]
    [SerializeField] private OptionsUI optionsUI;
    [SerializeField] private GameObject levelsUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

>>>>>>> Stashed changes
    private void Start()
    {

        quitButton.onClick.AddListener(QuitGame);
    }

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
    private void QuitGame()
    {
        Application.Quit();
    }
}
