using UnityEngine;

public class GridTileState : MonoBehaviour
{
    [Tooltip("If checked, this tile can NEVER have a trap placed on it, and always shows the red 'not allowed' highlight.")]
    public bool isBlocked = false;

    public GridObject currentObject { get; private set; }

    public void PlaceObject(GridObject obj)
    {
        if (currentObject != null) {
            if (obj is Soldier && currentObject is Trap) {
                currentObject.SteppedOn();
            }
        }
        currentObject = obj;
        currentObject.ObjectPlaced(this);
    }

    public void RemoveObject()
    {
        if (currentObject == null) return;

        currentObject.ObjectRemoved(this);
        Destroy(currentObject);
        currentObject = null;
    }
}