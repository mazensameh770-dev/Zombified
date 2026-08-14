using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Stars")]
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar;

    [Header("Win Info")]
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("References")]
    [SerializeField] private CameraPhaseController cameraPhaseController;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject winPanel;

    [Header("Extra gameplay UI to hide while Win is showing")]
    [SerializeField] private GameObject[] gameplayUIToHide;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private Image leftStar;
    private Image middleStar;
    private Image rightStar;

    private void Awake()
    {
        winPanel.SetActive(false);

        leftStar = FindStar("L_Star");
        middleStar = FindStar("M_Star");
        rightStar = FindStar("R_Star");
    }

    private void Start()
    {
        retryButton.onClick.AddListener(HandleRetry);
        nextLevelButton.onClick.AddListener(HandleNextLevel);
        mainMenuButton.onClick.AddListener(HandleMainMenu);

        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelWon += Show;
    }

    public void Show(float timeSpent, int starCount)
    {
        SetExtraGameplayUIVisible(false);

        UpdateTimeText(timeSpent);
        UpdateStars(starCount);

        winPanel.SetActive(true);
    }

    private void UpdateStars(int starCount)
    {
        UpdateStar(leftStar, starCount >= 1);
        UpdateStar(middleStar, starCount >= 2);
        UpdateStar(rightStar, starCount >= 3);
    }

    private void UpdateStar(Image star, bool filled)
    {
        if (star == null)
            return;

        star.sprite = filled ? filledStar : emptyStar;
    }

    private void UpdateTimeText(float timeSpent)
    {
        int minutes = Mathf.FloorToInt(timeSpent / 60f);
        int seconds = Mathf.FloorToInt(timeSpent % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private void HandleRetry()
    {
        Time.timeScale = 1f;

        ClearAllTraps();

        fadeUI.Hide();

        SetExtraGameplayUIVisible(true);

        winPanel.SetActive(false);

        GameManager.Instance.RestartCurrentLevel();

        cameraPhaseController.GoToLevel(
            cameraPhaseController.CurrentLevelIndex
        );

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }

    private void HandleNextLevel()
    {
        Time.timeScale = 1f;

        ClearAllTraps();

        fadeUI.Hide();

        SetExtraGameplayUIVisible(true);

        winPanel.SetActive(false);

        cameraPhaseController.OnNextLevelButtonPressed();

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }

    private void HandleMainMenu()
    {
        Time.timeScale = 1f;

        ClearAllTraps();

        fadeUI.Hide();

        winPanel.SetActive(false);

        cameraPhaseController.ReturnToMainMenuView();

        mainMenuUI.SetActive(true);

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.ClearSelection();
        }
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

    private void SetExtraGameplayUIVisible(bool visible)
    {
        foreach (GameObject ui in gameplayUIToHide)
        {
            if (ui != null)
                ui.SetActive(visible);
        }
    }

    private Image FindStar(string starName)
    {
        Transform star = FindChildRecursive(winPanel.transform, starName);

        if (star == null)
        {
            Debug.LogWarning(
                $"Could not find {starName} inside {winPanel.name}"
            );

            return null;
        }

        Image image = star.GetComponent<Image>();

        if (image == null)
        {
            Debug.LogWarning(
                $"{starName} does not have an Image component."
            );
        }

        return image;
    }

    private Transform FindChildRecursive(
        Transform parent,
        string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform result =
                FindChildRecursive(child, childName);

            if (result != null)
                return result;
        }

        return null;
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(HandleRetry);
        nextLevelButton.onClick.RemoveListener(HandleNextLevel);
        mainMenuButton.onClick.RemoveListener(HandleMainMenu);

        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelWon -= Show;
    }
}