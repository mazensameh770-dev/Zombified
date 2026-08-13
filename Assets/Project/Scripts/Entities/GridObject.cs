using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    [Header("Grid Object Settings")]
    protected GridTile currentGridTile;
    [SerializeField] protected int range;

    protected virtual void Start()
    {
    }
    //private void OnDestroy() {
    //    GameManager.Instance.OnNextTurn -= PlayNextAction;
    //    GameManager.Instance.OnSimulationEnd -= ResetObject;
    //}

    public void PlayTurn()
    {
        PlayNextAction();
    }

    public void ResetForLevel()
    {
        ResetObject();
    }

    public GridTile GetCurrentTile() {
        return currentGridTile;
    }
    public virtual void ObjectPlaced(GridTile tile) {
        currentGridTile = tile;
        transform.position = tile.transform.position;
    }
    public virtual void ObjectRemoved(GridTile tile) {
        currentGridTile = null;
    }
    protected virtual void PlayNextAction() {
        // Default implementation does nothing
    }
    protected virtual void ResetObject() {

    }
    public virtual void SteppedOn(GridObject soldier) {
        // Default implementation does nothing
    }
    public virtual void TakeDamage(int damage) {

    }

    public int getRange() {
        return range;
    }

    public static void StartSearching(GridTile startTile, int range, Action<GridTile> action) {
        if (startTile == null || range <= 0 || action == null) return;

        // Use a local HashSet for O(1) contains checks and to avoid static shared state.
        var visited = new HashSet<GridTile>();
        var queue = new Queue<(GridTile tile, int remainingRange)>();
        queue.Enqueue((startTile, range));

        while (queue.Count > 0) {
            var (tile, remaining) = queue.Dequeue();
            if (tile == null || remaining < 0) continue;

            // visited.Add returns false if already present -> skip duplicates
            if (!visited.Add(tile)) continue;

            // Skip calling the action for the starting tile only
            if (!ReferenceEquals(tile, startTile)) {
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
}