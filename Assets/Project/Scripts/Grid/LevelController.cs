using System;
using UnityEngine;

[Serializable]
public class TrapLimit
{
    public TrapCardData trapData;
    public int count;
}

public class LevelController : MonoBehaviour
{
    [SerializeField] private int simulationTurns = 5;

    [Header("Star Rating")]
    [SerializeField] private float threeStarTime = 20f;
    [SerializeField] private float twoStarTime = 40f;

    [Header("Traps Limits")]
    [SerializeField] private TrapLimit[] trapLimits;

    public int SimulationTurns => simulationTurns;
    public TrapLimit[] TrapLimits => trapLimits;

    private void OnEnable()
    {
        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }

    public GridObject[] GetGridObjects()
    {
        return GetComponentsInChildren<GridObject>(true);
    }

    public int CalculateStars(float time)
    {
        if (time <= threeStarTime)
            return 3;

        if (time <= twoStarTime)
            return 2;

        return 1;
    }

    public void ResetLevel()
    {
        GridObject[] objects = GetGridObjects();

        foreach (GridObject obj in objects)
        {
            if (obj != null)
                obj.ResetForLevel();
        }

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }

    public bool HasWon()
    {
        Soldier[] soldiers = GetComponentsInChildren<Soldier>(true);

        if (soldiers.Length == 0)
            return false;

        foreach (Soldier soldier in soldiers)
        {
            if (soldier.IsAlive())
                return false;
        }

        return true;
    }

    public int GetTrapCount(TrapCardData data)
    {
        if (trapLimits == null || data == null) return 0;

        int maxLimit = 0;
        bool found = false;

        foreach (var limit in trapLimits)
        {
            if (limit.trapData == data)
            {
                maxLimit = limit.count;
                found = true;
                break;
            }
        }

        if (!found) return 0;

        Trap[] placedTraps = GetComponentsInChildren<Trap>();
        int placedCount = 0;

        foreach (Trap trap in placedTraps)
        {
            if (trap != null && trap.gameObject.activeInHierarchy && trap.GetCurrentTile() != null && trap.TrapData == data)
            {
                placedCount++;
            }
        }

        return Mathf.Max(0, maxLimit - placedCount);
    }
}