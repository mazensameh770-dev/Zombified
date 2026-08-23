using System.Collections;
using UnityEngine;

public class CardDeckSpreader : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private RectTransform[] cards;
    [SerializeField] private float cardWidth = 110f;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private float verticalOffset = 40f; 

    [Header("Starting Fan Pose (like a hand of playing cards)")]
    [SerializeField] private float fanAngleStep = 8f;   
    [SerializeField] private float fanOffsetStep = 6f;  
    [SerializeField] private float fanVerticalPosition = 0f; 

    [Header("Animation")]
    [SerializeField] private float staggerDelay = 0.08f;   
    [SerializeField] private float moveDuration = 0.35f;   
    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (playOnStart) PlayFanReveal();
    }

    public void PlayFanReveal()
    {
        SoundManager.Instance.PlayCardShuffle();
        StopAllCoroutines();
        SetInitialFanPose();
        StartCoroutine(SpreadRoutine());
    }

    private void SetInitialFanPose()
    {
        int count = cards.Length;
        float startAngle = -fanAngleStep * (count - 1) / 2f;
        float startOffset = -fanOffsetStep * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            cards[i].anchoredPosition = new Vector2(startOffset + i * fanOffsetStep, fanVerticalPosition);
            cards[i].localRotation = Quaternion.Euler(0f, 0f, -(startAngle + i * fanAngleStep));
        }
    }

    public void SpreadCards()
    {
        StartCoroutine(SpreadRoutine());
    }

    private IEnumerator SpreadRoutine()
    {
        Vector2[] targetPositions = CalculateRowPositions();

        for (int i = 0; i < cards.Length; i++)
        {
            StartCoroutine(MoveCard(cards[i], targetPositions[i]));
            yield return new WaitForSeconds(staggerDelay);
        }
    }

    private IEnumerator MoveCard(RectTransform card, Vector2 targetPosition)
    {
        Vector2 startPosition = card.anchoredPosition;
        Quaternion startRotation = card.localRotation;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(Mathf.Clamp01(elapsed / moveDuration));
            card.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            card.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, t);
            yield return null;
        }

        card.anchoredPosition = targetPosition;
        card.localRotation = Quaternion.identity;
    }

    private Vector2[] CalculateRowPositions()
    {
        int count = cards.Length;
        float totalWidth = count * cardWidth + (count - 1) * spacing;
        float startX = -totalWidth / 2f + cardWidth / 2f;

        Vector2[] positions = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            positions[i] = new Vector2(startX + i * (cardWidth + spacing), verticalOffset);
        }

        return positions;
    }
}