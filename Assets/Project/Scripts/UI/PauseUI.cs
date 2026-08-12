using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [Header("UI")]
    [SerializeField] private GameObject optionsUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(OpenOptions);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void PauseGame()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        fadeUI.Hide();
    }

    private void OpenOptions()
    {
        fadeUI.Hide();

        optionsUI.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnDestroy()
    {
        pauseButton.onClick.RemoveListener(PauseGame);
        resumeButton.onClick.RemoveListener(ResumeGame);
        optionsButton.onClick.RemoveListener(OpenOptions);
        mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);

        Time.timeScale = 1f;
    }
}