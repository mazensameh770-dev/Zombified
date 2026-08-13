using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    [Header("References")]
    [SerializeField] private CameraPhaseController cameraPhaseController;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Extra gameplay UI to hide while Win is showing")]
    [Tooltip("Drag in any buttons/UI NOT already managed by CameraPhaseController - e.g. Simulate, Clear, BackToGame, RemoveTraps. CardPar, the Put-Traps/Back buttons, Time, Stars and the Pause button are already handled automatically and don't need to be added here.")]
    [SerializeField] private GameObject[] gameplayUIToHide;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private void Start()
    {
        retryButton.onClick.AddListener(HandleRetry);
        nextLevelButton.onClick.AddListener(HandleNextLevel);
        mainMenuButton.onClick.AddListener(HandleMainMenu);
    }

    public void Show()
    {
        SetExtraGameplayUIVisible(false);
        gameObject.SetActive(true); 
    }

    private void HandleRetry()
    {
        fadeUI.Hide();
        SetExtraGameplayUIVisible(true);
        cameraPhaseController.GoToLevel(cameraPhaseController.CurrentLevelIndex);
    }

    private void HandleNextLevel()
    {
        fadeUI.Hide();
        SetExtraGameplayUIVisible(true);
        cameraPhaseController.OnNextLevelButtonPressed();
    }

    private void HandleMainMenu()
    {
        fadeUI.Hide();
        cameraPhaseController.ReturnToMainMenuView();
        mainMenuUI.SetActive(true);
    }

    private void SetExtraGameplayUIVisible(bool visible)
    {
        foreach (GameObject ui in gameplayUIToHide)
        {
            if (ui != null) ui.SetActive(visible);
        }
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(HandleRetry);
        nextLevelButton.onClick.RemoveListener(HandleNextLevel);
        mainMenuButton.onClick.RemoveListener(HandleMainMenu);
    }
}