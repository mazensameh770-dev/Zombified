using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] private LevelController[] levels;

    //public event Action OnNextTurn;
    //public event Action OnSimulationEnd;

    private LevelController currentLevel;
    public Transform CurrentLevelRoot => currentLevel != null ? currentLevel.transform : null;
    private Coroutine simulationRoutine;

    public int CurrentLevelIndex { get; private set; } = -1;

    //private GridObject[] LevelObjects;
    //[SerializeField] private int simulationTurns;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogWarning($"Invalid level index: {levelIndex}");
            return;
        }

        if (simulationRoutine != null)
        {
            StopCoroutine(simulationRoutine);
            simulationRoutine = null;
        }

        CurrentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex];

        currentLevel.ResetLevel();
    }


    public void Simulate()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No level selected.");
            return;
        }

        if (simulationRoutine != null)
            return;

        simulationRoutine = StartCoroutine(SimulationRoutine());
    }

    private IEnumerator SimulationRoutine()
    {
        GridObject[] objects;

        for (int turn = 0; turn < currentLevel.SimulationTurns; turn++)
        {
            objects = currentLevel.GetGridObjects();

            foreach (GridObject obj in objects)
            {
                if (obj != null && obj.gameObject.activeInHierarchy)
                    obj.PlayTurn();
            }

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        currentLevel.ResetLevel();

        simulationRoutine = null;
    }


    //public async void Simulate() {
    //    //LevelObjects = FindObjectsByType<GridObject>(FindObjectsSortMode.None);
    //    for (int i = 0; i < simulationTurns; i++) {
    //        /*
    //        foreach (var obj in LevelObjects) {
    //            obj.PlayNextAction();
    //        }
    //        */
    //        OnNextTurn?.Invoke();
    //        await Task.Delay(1000);
    //    }
    //    await Task.Delay(2000);
    //    OnSimulationEnd?.Invoke();
    //    //ResetLevel();
    //}
    private void ResetLevel() {
        /*
        print("Game over, resetting");
        foreach (var obj in LevelObjects) {
            obj.gameObject.SetActive(true);
            if (obj is Soldier) {
                (obj as Soldier).ResetSoldier();
            }
        }
        */
    }
}
