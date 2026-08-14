using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Level Buttons (index 0 = Level 1, index 1 = Level 2, ...)")]
    [SerializeField] private Button[] levelButtons;

    [SerializeField] private CameraPhaseController cameraPhaseController;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private LevelButtonStars[] levelButtonStars;

    private void Awake()
    {
        levelButtonStars =
            GetComponentsInChildren<LevelButtonStars>(true);
    }

    private void OnEnable()
    {
        StartCoroutine(RefreshNextFrame());
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;

        RefreshLevels();
    }

    public void RefreshLevels()
    {
        foreach (LevelButtonStars levelButton in levelButtonStars)
        {
            if (levelButton != null)
                levelButton.UpdateLevel();
        }
    }

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i;

            levelButtons[i].onClick.AddListener(
                () => SelectLevel(levelIndex)
            );
        }
    }

    private void SelectLevel(int levelIndex)
    {
        fadeUI.Hide();
        cameraPhaseController.GoToLevel(levelIndex);
    }

    private void GoBack()
    {
        fadeUI.Hide();
        mainMenuUI.SetActive(true);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(GoBack);

        foreach (Button button in levelButtons)
        {
            if (button != null)
                button.onClick.RemoveAllListeners();
        }
    }
}