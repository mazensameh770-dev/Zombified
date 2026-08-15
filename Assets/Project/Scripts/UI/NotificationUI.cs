using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance { get; private set; }

    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayDuration = 2.5f;

    private Sequence currentTween;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (notificationPanel == null || messageText == null) return;

        messageText.text = message;

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        notificationPanel.SetActive(true);

        CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = notificationPanel.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        notificationPanel.transform.localScale = Vector3.one * 0.8f;

        currentTween = DOTween.Sequence();
        currentTween.Append(canvasGroup.DOFade(1f, 0.2f));
        currentTween.Join(notificationPanel.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        currentTween.AppendInterval(displayDuration);
        currentTween.Append(canvasGroup.DOFade(0f, 0.3f));
        currentTween.OnComplete(() => notificationPanel.SetActive(false));
    }
}