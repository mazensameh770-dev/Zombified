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

    [Header("Phase Buttons")]
    [SerializeField] private GameObject putTrapsButton; 
    [SerializeField] private GameObject backButton;      
    [SerializeField] private GameObject nextLevelButton; 

    [Header("Hooks (optional, clears placement state on transitions)")]
    [SerializeField] private CardSelectionManager cardSelectionManager;

    private Coroutine activeTransition;
    private bool isInSetupPhase;
    private bool isTransitioning;
    private int currentLevelIndex;

    public static bool IsInSetupPhase { get; private set; }

    private LevelViewpoints CurrentLevel => levels[currentLevelIndex];

    private void Start()
    {
        SetCardBarVisible(false);
        SetButtonsVisible(putTrapsVisible: false, backVisible: false);
        nextLevelButton?.SetActive(false);
        IsInSetupPhase = false;
    }

    public void GoToLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogWarning($"[CameraPhaseController] Level index {levelIndex} is out of range.");
            return;
        }

        if (activeTransition != null) StopCoroutine(activeTransition);
        isTransitioning = false;

        currentLevelIndex = levelIndex;
        SnapToLevelStart();
    }

    public void ShowNextLevelButton()
    {
        if (currentLevelIndex + 1 >= levels.Length)
        {
            Debug.Log("[CameraPhaseController] That was the last level - no Next Level button to show.");
            return;
        }

        nextLevelButton?.SetActive(true);
    }

    public void OnNextLevelButtonPressed()
    {
        nextLevelButton?.SetActive(false);
        GoToLevel(currentLevelIndex + 1);
    }

    private void SnapToLevelStart()
    {
        isInSetupPhase = false;
        IsInSetupPhase = false;

        transform.position = CurrentLevel.isometricViewPoint.position;
        transform.rotation = CurrentLevel.isometricViewPoint.rotation;

        SetCardBarVisible(false);
        SetButtonsVisible(putTrapsVisible: true, backVisible: false);
        nextLevelButton?.SetActive(false);
        cardSelectionManager?.ClearSelection();
    }

    public void EnterSetupPhase()
    {
        if (isInSetupPhase || isTransitioning) return;
        isInSetupPhase = true;
        IsInSetupPhase = true;

        SetButtonsVisible(putTrapsVisible: false, backVisible: false); 
        StartTransition(CurrentLevel.setupViewPoint, onComplete: () =>
        {
            SetCardBarVisible(true);
            SetButtonsVisible(putTrapsVisible: false, backVisible: true);
            cardDeckSpreader?.PlayFanReveal(); 
        });
    }

    public void ReturnToIsometricPhase()
    {
        if (!isInSetupPhase || isTransitioning) return;
        isInSetupPhase = false;
        IsInSetupPhase = false;

        SetCardBarVisible(false);
        SetButtonsVisible(putTrapsVisible: false, backVisible: false); 
        cardSelectionManager?.ClearSelection();

        StartTransition(CurrentLevel.isometricViewPoint, onComplete: () =>
        {
            SetButtonsVisible(putTrapsVisible: true, backVisible: false);
        });
    }

    private void SetButtonsVisible(bool putTrapsVisible, bool backVisible)
    {
        if (putTrapsButton != null) putTrapsButton.SetActive(putTrapsVisible);
        if (backButton != null) backButton.SetActive(backVisible);
    }

    private void SetCardBarVisible(bool visible)
    {
        if (cardBarRoot != null) cardBarRoot.SetActive(visible);
    }

    private void StartTransition(Transform target, Action onComplete)
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionRoutine(target, onComplete));
    }

    private IEnumerator TransitionRoutine(Transform target, Action onComplete)
    {
        isTransitioning = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(Mathf.Clamp01(elapsed / transitionDuration));

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
        isTransitioning = false;
        activeTransition = null;
        onComplete?.Invoke();
    }
}