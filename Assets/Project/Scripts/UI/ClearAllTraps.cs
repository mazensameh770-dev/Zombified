using UnityEngine;
using UnityEngine.UI;

public class ClearAllTrapsButton : MonoBehaviour
{
    [SerializeField] private Button clearButton;

    private void Awake()
    {
        clearButton.onClick.AddListener(ClearAll);
    }

    private void ClearAll()
    {
        if (!CameraPhaseController.IsInSetupPhase) return;

        Trap[] traps = GameManager.Instance.CurrentLevelRoot.GetComponentsInChildren<Trap>();

        foreach (Trap trap in traps)
        {
            GridTile tile = trap.GetCurrentTile();
            if (tile != null) tile.RemoveObject();
        }

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }

    private void OnDestroy()
    {
        clearButton.onClick.RemoveListener(ClearAll);
    }
}