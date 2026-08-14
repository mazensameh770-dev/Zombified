using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class LevelViewpoints
{
    public string levelName = "Level";
    public Transform isometricViewPoint;
    public Transform setupViewPoint;
}

public class CameraPhaseController : MonoBehaviour
{
    [Header("Levels (index 0 = Level 1, index 1 = Level 2, ...)")]
    [SerializeField] private LevelViewpoints[] levels;

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 1.2f;
    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("UI To Show/Hide During Setup")]
    [SerializeField] private GameObject cardBarRoot;
    [SerializeField] private CardDeckSpreader cardDeckSpreader;

    [Header("Phase Buttons (any number of buttons per phase)")]
    [SerializeField] private GameObject[] isometricPhaseButtons;
    [SerializeField] private GameObject[] setupPhaseButtons;

    [Header("Gameplay HUD (shown for the whole level, hidden at Main Menu)")]
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private GameObject starsUI;
    [SerializeField] private GameObject pauseButtonUI;

    [Header("Hooks (optional, clears placement state on transitions)")]
    [SerializeField] private CardSelectionManager cardSelectionManager;

    private Coroutine activeTransition;
    private bool isInSetupPhase;
    private bool isTransitioning;
    private int currentLevelIndex;

    private Vector3 homePosition;
    private Quaternion homeRotation;

    public static bool IsInSetupPhase { get; private set; }
    public int CurrentLevelIndex => currentLevelIndex;

    private LevelViewpoints CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        homePosition = transform.position;
        homeRotation = transform.rotation;
    }

    private void Start()
    {
        SetCardBarVisible(false);
        SetPhaseButtonsVisible(isometricVisible: false, setupVisible: false);
        SetGameplayHUDVisible(false);
        IsInSetupPhase = false;
    }

    public void GoToLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogWarning($"[CameraPhaseController] Level index {levelIndex} is out of range.");
            return;
        }
        currentLevelIndex = levelIndex;

        GameManager.Instance.SetCurrentLevel(levelIndex);
        isInSetupPhase = false;
        IsInSetupPhase = false;

        SetCardBarVisible(false);
        SetPhaseButtonsVisible(isometricVisible: false, setupVisible: false);
        cardSelectionManager?.ClearSelection();

        StartTransition(CurrentLevel.isometricViewPoint, onComplete: () =>
        {
            SetPhaseButtonsVisible(isometricVisible: true, setupVisible: false);
            SetGameplayHUDVisible(true);
        });
    }

    public void ReturnToMainMenuView(Action onComplete = null)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopSimulationAndReset();
        }

        timeUI?.ResetTimer();
        isInSetupPhase = false;
        IsInSetupPhase = false;

        SetCardBarVisible(false);
        SetPhaseButtonsVisible(isometricVisible: false, setupVisible: false);
        cardSelectionManager?.ClearSelection();
        SetGameplayHUDVisible(false);

        StartTransitionToHome(onComplete: () =>
        {
            onComplete?.Invoke();
        });
    }

    public void ShowNextLevelButton()
    {
        if (currentLevelIndex + 1 >= levels.Length)
        {
            Debug.Log("[CameraPhaseController] That was the last level - no Next Level button to show.");
            return;
        }
    }

    public void OnNextLevelButtonPressed()
    {
        timeUI?.ResetTimer();
        GoToLevel(currentLevelIndex + 1);
    }

    public void EnterSetupPhase()
    {
        if (isInSetupPhase || isTransitioning) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopSimulationAndReset();
        }

        isInSetupPhase = true;
        IsInSetupPhase = true;

        SetPhaseButtonsVisible(isometricVisible: false, setupVisible: false);
        StartTransition(CurrentLevel.setupViewPoint, onComplete: () =>
        {
            SetCardBarVisible(true);
            SetPhaseButtonsVisible(isometricVisible: false, setupVisible: true);
            cardDeckSpreader?.PlayFanReveal();
        });
    }

    public void ReturnToIsometricPhase()
    {
        if (!isInSetupPhase || isTransitioning) return;
        isInSetupPhase = false;
        IsInSetupPhase = false;

        SetCardBarVisible(false);
        SetPhaseButtonsVisible(isometricVisible: false, setupVisible: false);
        cardSelectionManager?.ClearSelection();

        StartTransition(CurrentLevel.isometricViewPoint, onComplete: () =>
        {
            SetPhaseButtonsVisible(isometricVisible: true, setupVisible: false);
        });
    }

    private void SetPhaseButtonsVisible(bool isometricVisible, bool setupVisible)
    {
        SetActiveAll(isometricPhaseButtons, isometricVisible);
        SetActiveAll(setupPhaseButtons, setupVisible);
    }

    private void SetActiveAll(GameObject[] objects, bool visible)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects)
        {
            if (obj != null) obj.SetActive(visible);
        }
    }

    private void SetCardBarVisible(bool visible)
    {
        if (cardBarRoot != null) cardBarRoot.SetActive(visible);
    }

    private void SetGameplayHUDVisible(bool visible)
    {
        if (timeUI != null) timeUI.gameObject.SetActive(visible);
        if (starsUI != null) starsUI.SetActive(visible);
        if (pauseButtonUI != null) pauseButtonUI.SetActive(visible);
    }

    private void StartTransition(Transform target, Action onComplete)
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionRoutine(target.position, target.rotation, onComplete));
    }

    private void StartTransitionToHome(Action onComplete)
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionRoutine(homePosition, homeRotation, onComplete));
    }

    private IEnumerator TransitionRoutine(Vector3 targetPosition, Quaternion targetRotation, Action onComplete)
    {
        isTransitioning = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(Mathf.Clamp01(elapsed / transitionDuration));

            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isTransitioning = false;
        activeTransition = null;
        onComplete?.Invoke();
    }
}