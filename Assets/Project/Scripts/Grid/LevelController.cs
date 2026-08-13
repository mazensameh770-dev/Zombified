using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int simulationTurns = 5;

    public int SimulationTurns => simulationTurns;

    public GridObject[] GetGridObjects()
    {
        return GetComponentsInChildren<GridObject>(true);
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
}
