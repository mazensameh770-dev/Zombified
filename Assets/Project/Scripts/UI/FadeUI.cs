using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.25f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;

        StartFade(1f);
    }

    public void Hide()
    {
        StartFade(0f, true);
    }

    private void StartFade(float targetAlpha, bool disableAfter = false)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(targetAlpha, disableAfter));
    }

    private IEnumerator Fade(float targetAlpha, bool disableAfter)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha >= 1f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else if (disableAfter)
        {
            gameObject.SetActive(false);
        }
    }
}