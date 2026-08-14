using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int simulationTurns = 5;

    [Header("Star Rating")]
    [SerializeField] private float threeStarTime = 20f;
    [SerializeField] private float twoStarTime = 40f;
    public int SimulationTurns => simulationTurns;

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
}
