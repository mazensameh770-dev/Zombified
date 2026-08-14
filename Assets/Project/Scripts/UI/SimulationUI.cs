using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour
{
    [SerializeField] private Button simulateButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button editButton;

    private void Start()
    {
        if (simulateButton != null)
            simulateButton.onClick.AddListener(OnSimulateClicked);

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetClicked);
            resetButton.gameObject.SetActive(false);
        }

        if (editButton != null)
            editButton.onClick.AddListener(OnEditClicked);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSimulationStarted += HandleSimulationStarted;
            GameManager.Instance.OnLevelReset += HandleLevelReset;
        }
    }

    private void OnSimulateClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Simulate();
        }
    }

    private void OnResetClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopSimulationAndReset();
        }
    }

    private void OnEditClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopSimulationAndReset();
        }
    }

    private void HandleSimulationStarted()
    {
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
        }
    }

    private void HandleLevelReset()
    {
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (simulateButton != null)
            simulateButton.onClick.RemoveListener(OnSimulateClicked);

        if (resetButton != null)
            resetButton.onClick.RemoveListener(OnResetClicked);

        if (editButton != null)
            editButton.onClick.RemoveListener(OnEditClicked);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSimulationStarted -= HandleSimulationStarted;
            GameManager.Instance.OnLevelReset -= HandleLevelReset;
        }
    }
}