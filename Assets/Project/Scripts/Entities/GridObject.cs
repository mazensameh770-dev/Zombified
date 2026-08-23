using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    [Header("Grid Object Settings")]
    protected GridTile currentGridTile;
    [SerializeField] protected int range;
    public GridTile CurrentGridTile => currentGridTile;

    protected virtual void Start()
    {
    }

    public void PlayTurn()
    {
        PlayNextAction();
    }

    public void ResetForLevel()
    {
        ResetObject();
    }

    public GridTile GetCurrentTile()
    {
        return currentGridTile;
    }

    public virtual void ObjectPlaced(GridTile tile)
    {
        currentGridTile = tile;
        transform.position = tile.transform.position;
    }

    public virtual void ObjectRemoved(GridTile tile)
    {
        currentGridTile = null;
    }

    protected virtual void PlayNextAction()
    {
    }

    protected virtual void ResetObject()
    {

    }

    public virtual void SteppedOn(GridObject soldier)
    {
    }

    public virtual void TakeDamage(int damage)
    {

    }

    public int getRange()
    {
        return range;
    }

    public static void StartSearching(GridTile startTile, int range, Action<GridTile> action)
    {
        if (startTile == null || range <= 0 || action == null) return;

        var visited = new HashSet<GridTile>();
        var queue = new Queue<(GridTile tile, int remainingRange)>();
        queue.Enqueue((startTile, range));

        while (queue.Count > 0)
        {
            var (tile, remaining) = queue.Dequeue();
            if (tile == null || remaining < 0) continue;

            if (!visited.Add(tile)) continue;

            if (!ReferenceEquals(tile, startTile))
            {
                action(tile);
            }

            int nextRange = remaining - 1;
            if (nextRange < 0) continue;

            var front = tile.GetFront();
            var back = tile.GetBack();
            var right = tile.GetRight();
            var left = tile.GetLeft();

            if (front != null) queue.Enqueue((front, nextRange));
            if (back != null) queue.Enqueue((back, nextRange));
            if (right != null) queue.Enqueue((right, nextRange));
            if (left != null) queue.Enqueue((left, nextRange));
        }
    }

    public Queue<GridTile> GetTilesInRange(GridTile startTile, int range)
    {
        var tilesInRange = new Queue<GridTile>();
        StartSearching(startTile, range, tile => tilesInRange.Enqueue(tile));
        return tilesInRange;
    }
}