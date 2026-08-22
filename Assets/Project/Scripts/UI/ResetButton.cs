using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject resetPanel;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        resetPanel.SetActive(true);
    }
}
