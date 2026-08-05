using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TrapCardUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Data")]
    [SerializeField] private TrapCardData trapData;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private int startingQuantity = 3; 

    [Header("Highlight Animation")]
    [SerializeField] private float selectedScale = 1.15f;
    [SerializeField] private float selectedYOffset = 25f;
    [SerializeField] private float animationDuration = 0.15f;

    private RectTransform rectTransform;
    private Vector2 restPosition;   
    private Vector3 restScale;
    private Coroutine activeAnimation;
    private int remainingQuantity;

    public TrapCardData TrapData => trapData;
    public bool HasTrapsRemaining => remainingQuantity > 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        remainingQuantity = startingQuantity;
        UpdateQuantityText();

        if (nameText != null) nameText.text = trapData.trapName;
    }

    private void Start()
    {
        CardSelectionManager.Instance.RegisterCard(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasTrapsRemaining) return;
        CardSelectionManager.Instance.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            restPosition = rectTransform.anchoredPosition;
            restScale = rectTransform.localScale;

            Vector2 target = restPosition + Vector2.up * selectedYOffset;
            AnimateTo(target, restScale * selectedScale);
        }
        else
        {
            AnimateTo(restPosition, restScale);
        }
    }

    public void ConsumeOne()
    {
        remainingQuantity = Mathf.Max(0, remainingQuantity - 1);
        UpdateQuantityText();
    }

    public void AddOne()
    {
        remainingQuantity++;
        UpdateQuantityText();
    }

    public void SetQuantity(int newQuantity)
    {
        remainingQuantity = Mathf.Max(0, newQuantity);
        UpdateQuantityText();
    }

    public int GetQuantity()
    {
        return remainingQuantity;
    }

    private void UpdateQuantityText()
    {
        if (quantityText != null) quantityText.text = remainingQuantity.ToString();
    }

    private void AnimateTo(Vector2 targetPosition, Vector3 targetScale)
    {
        if (activeAnimation != null) StopCoroutine(activeAnimation);
        activeAnimation = StartCoroutine(AnimateRoutine(targetPosition, targetScale));
    }

    private IEnumerator AnimateRoutine(Vector2 targetPosition, Vector3 targetScale)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector3 startScale = rectTransform.localScale;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        rectTransform.localScale = targetScale;
        activeAnimation = null;
    }
}