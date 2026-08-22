using UnityEngine;
using UnityEngine.UI;

public class ResetUI : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button clearButton;

    private void Start()
    {
        cancelButton.onClick.AddListener(CloseResetPanel);
        clearButton.onClick.AddListener(ClearGameData);
    }

    private void CloseResetPanel()
    {
        gameObject.SetActive(false);
    }

    private void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        CloseResetPanel();
    }
}
