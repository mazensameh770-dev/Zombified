using UnityEngine;
using UnityEngine.UI;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        fadeUI.FadeOut();

        Invoke(nameof(ShowMainMenu), fadeUI.fadeDuration);
    }

    private void ShowMainMenu()
    {
        gameObject.SetActive(false);

        mainMenuUI.SetActive(true);

        FadeUI mainMenuFade = mainMenuUI.GetComponent<FadeUI>();
        mainMenuFade.FadeIn();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(GoBack);
    }
}