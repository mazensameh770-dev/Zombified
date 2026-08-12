using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private PauseUI pauseUI;

    private void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        pauseUI.PauseGame();
    }

    private void OnDestroy()
    {
        pauseButton.onClick.RemoveListener(PauseGame);
    }
}