using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] private LevelController[] levels;

    [SerializeField] private TimeUI timeUI;

    private LevelController currentLevel;
    public LevelController CurrentLevel => currentLevel;
    public Transform CurrentLevelRoot => currentLevel != null ? currentLevel.transform : null;
    private Coroutine simulationRoutine;

    public int CurrentLevelIndex { get; private set; } = -1;

    public event Action<float, int> OnLevelWon;

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
        for (int turn = 0; turn < currentLevel.SimulationTurns; turn++)
        {
            GridObject[] objects = currentLevel.GetGridObjects();

            foreach (GridObject obj in objects)
            {
                if (obj != null && obj.gameObject.activeInHierarchy)
                    obj.PlayTurn();
            }

            yield return new WaitForSeconds(1f);

            if (currentLevel.HasWon())
            {
                WinLevel();
                yield break;
            }
        }

        simulationRoutine = null;
    }

    public void RestartCurrentLevel()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No level selected.");
            return;
        }

        if (simulationRoutine != null)
        {
            StopCoroutine(simulationRoutine);
            simulationRoutine = null;
        }

        Time.timeScale = 1f;

        timeUI.ResetTimer();

        currentLevel.ResetLevel();
    }

    private void WinLevel()
    {
        simulationRoutine = null;

        Time.timeScale = 0f;

        float timeSpent = timeUI.GetTime();

        int stars = currentLevel.CalculateStars(timeSpent);

        LevelProgress.SaveStars(CurrentLevelIndex, stars);

        if (CurrentLevelIndex + 1 < levels.Length)
        {
            LevelProgress.UnlockLevel(CurrentLevelIndex + 1);
        }

        OnLevelWon?.Invoke(timeSpent, stars);
    }
}
