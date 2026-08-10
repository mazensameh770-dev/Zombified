using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    private GridTileState currentGridTile;
    [SerializeField] protected int range;
    
    public GridTileState GetCurrentTile() {
        return currentGridTile;
    }
    public virtual void ObjectPlaced(GridTileState tile) {
        currentGridTile = tile;
    }
    public virtual void ObjectRemoved(GridTileState tile) {
        currentGridTile = null;
    }
    public virtual void PlayNextAction() {
        // Default implementation does nothing
    }
    public virtual void SteppedOn() {
        // Default implementation does nothing
    }
}
