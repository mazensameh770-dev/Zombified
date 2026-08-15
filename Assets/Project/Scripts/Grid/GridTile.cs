using DG.Tweening;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [Tooltip("If checked, this tile can NEVER have a trap placed on it, and always shows the red 'not allowed' highlight.")]
    public bool isBlocked = false;

    private GridObject currentObject;

    public GridTile[] neighbors = new GridTile[4];

    public GridTile GetFront() => (neighbors != null && neighbors.Length > 0) ? neighbors[0] : null;
    public GridTile GetBack() => (neighbors != null && neighbors.Length > 1) ? neighbors[1] : null;
    public GridTile GetRight() => (neighbors != null && neighbors.Length > 2) ? neighbors[2] : null;
    public GridTile GetLeft() => (neighbors != null && neighbors.Length > 3) ? neighbors[3] : null;

    public void PlaceObject(GridObject obj, GridTile sourceTile = null)
    {
        if (obj == null) return;

        if (currentObject != null)
        {
            if (currentObject is Trap trap)
            {
                if (obj is Soldier)
                {
                    trap.SteppedOn(obj);
                    return;
                }
                else if (obj is Zombie)
                {
                    trap.DeactivateTrap();
                    currentObject = obj;
                    currentObject.ObjectPlaced(this);
                    trap.SteppedOn(obj);
                    return;
                }
            }
            else if (obj is Zombie && currentObject is Soldier)
            {
                GridObject soldierObj = currentObject;
                currentObject = null;
                ZombieManager.Instance.Zombifying(this, soldierObj, sourceTile);
                return;
            }
        }
        currentObject = obj;
        currentObject.ObjectPlaced(this);
    }

    public void RemoveObject(bool destroy = true)
    {
        if (currentObject == null) return;

        GridObject objToRemove = currentObject;
        currentObject = null;

        objToRemove.ObjectRemoved(this);
        if (destroy)
        {
            Destroy(objToRemove.gameObject);
        }
    }

    public void MoveObject(GridTile targetTile)
    {
        if (currentObject == null || targetTile == null) return;

        GridObject objToMove = currentObject;
        GridTile sourceTile = this;

        objToMove.transform.DOMove(targetTile.transform.position, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if (objToMove != null && objToMove.gameObject != null)
                {
                    objToMove.ObjectRemoved(sourceTile);
                    targetTile.PlaceObject(objToMove, sourceTile);
                    if (currentObject == objToMove) currentObject = null;
                }
            });
    }

    public GridObject GetCurrentObject() => currentObject;
}