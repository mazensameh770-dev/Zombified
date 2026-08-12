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
        fadeUI.Hide();

        mainMenuUI.SetActive(true);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(GoBack);
    }
}