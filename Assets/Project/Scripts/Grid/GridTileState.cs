using UnityEngine;

public class GridTileState : MonoBehaviour
{
    [Tooltip("If checked, this tile can NEVER have a trap placed on it, and always shows the red 'not allowed' highlight.")]
    public bool isBlocked = false;

    public GameObject PlacedTrap { get; private set; }
    public TrapCardData SourceCardData { get; private set; }

    public bool CanPlaceTrap()
    {
        return !isBlocked && PlacedTrap == null;
    }

    public void SetPlacedTrap(GameObject trap, TrapCardData sourceCardData)
    {
        PlacedTrap = trap;
        SourceCardData = sourceCardData;
    }

    public void RemoveTrap()
    {
        if (PlacedTrap == null) return;

        CardSelectionManager.Instance?.RestoreOne(SourceCardData);

        Destroy(PlacedTrap);
        PlacedTrap = null;
        SourceCardData = null;
    }
}