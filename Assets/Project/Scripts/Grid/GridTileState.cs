using UnityEngine;

public class GridTileState : MonoBehaviour
{
    [Tooltip("If checked, this tile can NEVER have a trap placed on it, and always shows the red 'not allowed' highlight.")]
    public bool isBlocked = false;

    public GameObject PlacedTrap { get; private set; }
    public TrapCardData SourceCardData { get; private set; }

    public GridTileState[] neighbors = new GridTileState[4];

    public GridTileState GetFront()
    {
        return (neighbors != null && neighbors.Length > 0) ? neighbors[0] : null;
    }

    public GridTileState GetBack()
    {
        return (neighbors != null && neighbors.Length > 1) ? neighbors[1] : null;
    }

    public GridTileState GetRight()
    {
        return (neighbors != null && neighbors.Length > 2) ? neighbors[2] : null;
    }

    public GridTileState GetLeft()
    {
        return (neighbors != null && neighbors.Length > 3) ? neighbors[3] : null;
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