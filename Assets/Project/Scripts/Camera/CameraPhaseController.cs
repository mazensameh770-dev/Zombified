using System.Collections;
using UnityEngine;

public class CameraPhaseController : MonoBehaviour
{
    [Header("View Points")]
    [SerializeField] private Transform isometricViewPoint;
    [SerializeField] private Transform setupViewPoint;

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 1.2f;
    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("UI To Show/Hide During Setup")]
    [SerializeField] private GameObject cardBarRoot;

    [Header("Phase Buttons")]
    [SerializeField] private GameObject putTrapsButton; 
    [SerializeField] private GameObject backButton;      

    [Header("Hooks (optional, clears placement state on exit)")]
    [SerializeField] private CardSelectionManager cardSelectionManager;

    private Coroutine activeTransition;
    private bool isInSetupPhase;
    private bool isTransitioning;

    public static bool IsInSetupPhase { get; private set; }

    private void Start()
    {
        transform.position = isometricViewPoint.position;
        transform.rotation = isometricViewPoint.rotation;
        SetCardBarVisible(false);
        SetButtonsVisible(putTrapsVisible: true, backVisible: false);
        IsInSetupPhase = false;
    }

    public void EnterSetupPhase()
    {
        if (isInSetupPhase || isTransitioning) return;
        isInSetupPhase = true;
        IsInSetupPhase = true;

        SetButtonsVisible(putTrapsVisible: false, backVisible: false); 
        StartTransition(setupViewPoint, onComplete: () =>
        {
            SetCardBarVisible(true);
            SetButtonsVisible(putTrapsVisible: false, backVisible: true);
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

        StartTransition(isometricViewPoint, onComplete: () =>
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

    private void StartTransition(Transform target, System.Action onComplete)
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionRoutine(target, onComplete));
    }

    private IEnumerator TransitionRoutine(Transform target, System.Action onComplete)
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