using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [Header("UI")]
    [SerializeField] private OptionsUI optionsUI;
    [SerializeField] private GameObject mainMenuUI;

    [Header("References")]
    [SerializeField] private CameraPhaseController cameraPhaseController;

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
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        fadeUI.Hide();
        gameObject.SetActive(false);
    }

    private void OpenOptions()
    {
        fadeUI.Hide();
        optionsUI.Open(gameObject);
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        ClearAllTraps();
        fadeUI.Hide();
        gameObject.SetActive(false);

        cameraPhaseController.ReturnToMainMenuView(() =>
        {
            mainMenuUI.SetActive(true);
        });
    }

    private void ClearAllTraps()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentLevelRoot != null)
        {
            Trap[] levelTraps = GameManager.Instance.CurrentLevelRoot.GetComponentsInChildren<Trap>(true);
            foreach (Trap trap in levelTraps)
            {
                if (trap == null) continue;
                GridTile tile = trap.GetCurrentTile();
                if (tile != null) tile.RemoveObject();
                Destroy(trap.gameObject);
            }
        }

        Trap[] sceneTraps = FindObjectsOfType<Trap>(true);
        foreach (Trap trap in sceneTraps)
        {
            if (trap == null) continue;
            GridTile tile = trap.GetCurrentTile();
            if (tile != null) tile.RemoveObject();
            Destroy(trap.gameObject);
        }
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