using DG.Tweening;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [Tooltip("If checked, this tile can NEVER have a trap placed on it, and always shows the red 'not allowed' highlight.")]
    public bool isBlocked = false;

    private GridObject currentObject;
    private GridObject temp;

    public GridTile[] neighbors = new GridTile[4];

    public GridTile GetFront()
    {
        return (neighbors != null && neighbors.Length > 0) ? neighbors[0] : null;
    }

    public GridTile GetBack()
    {
        return (neighbors != null && neighbors.Length > 1) ? neighbors[1] : null;
    }

    public GridTile GetRight()
    {
        return (neighbors != null && neighbors.Length > 2) ? neighbors[2] : null;
    }

    public GridTile GetLeft()
    {
        return (neighbors != null && neighbors.Length > 3) ? neighbors[3] : null;
    }

    //public void SetPlacedTrap(GameObject trap, TrapCardData sourceCardData)
    public void PlaceObject(GridObject obj)
    {
        if (currentObject != null) {
            if (obj is Soldier && currentObject is Trap) {
                currentObject.SteppedOn(obj);
                return;
            } else if (obj is Zombie && currentObject is Trap) {
                currentObject.SteppedOn(obj);
                temp = currentObject;
                // don't return
            } else if (obj is Zombie && currentObject is Soldier) {
                ZombieManager.Instance.Zombifying(this);
                temp = currentObject;
                // don't return
            }
        }
        currentObject = obj;
        currentObject.ObjectPlaced(this);
    }

    public void RemoveObject(bool destroy = true)
    {
        if (currentObject == null) return;

        currentObject.ObjectRemoved(this);
        if (destroy) {
            Destroy(currentObject.gameObject);
        }
        if (temp != null) {
            currentObject = temp;
            temp = null;
        } else currentObject = null;
    }
    public void MoveObject(GridTile targetTile)
    {
        currentObject.ObjectRemoved(this);
        currentObject.transform.DOMove(targetTile.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            targetTile.PlaceObject(currentObject);
            currentObject = null;
        });
    }
    public GridObject GetCurrentObject()
    {
        return currentObject;
    }
}