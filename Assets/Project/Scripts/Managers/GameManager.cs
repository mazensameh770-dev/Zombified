using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelController[] levels;

    [SerializeField] private TimeUI timeUI;

    private LevelController currentLevel;
    public LevelController CurrentLevel => currentLevel;
    public Transform CurrentLevelRoot => currentLevel != null ? currentLevel.transform : null;
    private Coroutine simulationRoutine;

    public int CurrentLevelIndex { get; private set; } = -1;
    public bool IsSimulating => simulationRoutine != null;

    public event Action<float, int> OnLevelWon;
    public event Action OnSimulationStarted;
    public event Action OnSimulationEnded;
    public event Action OnLevelReset;

    private void Awake()
    {
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

        Time.timeScale = 1f;
        StopSimulationOnly();

        CurrentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex];

        currentLevel.ResetLevel();
        OnLevelReset?.Invoke();
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

        Time.timeScale = 1f;
        OnSimulationStarted?.Invoke();
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
        OnSimulationEnded?.Invoke();
    }

    public void StopSimulationAndReset(bool resetTimer)
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        StopSimulationOnly();

        if (currentLevel != null)
        {
            currentLevel.ResetLevel();
        }

        if (timeUI != null && resetTimer)
        {
            timeUI.ResetTimer();
        }

        OnLevelReset?.Invoke();
    }

    private void StopSimulationOnly()
    {
        if (simulationRoutine != null)
        {
            StopCoroutine(simulationRoutine);
            simulationRoutine = null;
            OnSimulationEnded?.Invoke();
        }
    }

    public void RestartCurrentLevel()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No level selected.");
            return;
        }

        StopSimulationAndReset(true);
        Time.timeScale = 1f;
    }

    private void WinLevel()
    {
        StopSimulationOnly();

        Time.timeScale = 0f;

        float timeSpent = timeUI != null ? timeUI.GetTime() : 0f;

        int stars = currentLevel.CalculateStars(timeSpent);

        LevelProgress.SaveStars(CurrentLevelIndex, stars);

        if (CurrentLevelIndex + 1 < levels.Length)
        {
            LevelProgress.UnlockLevel(CurrentLevelIndex + 1);
        }

        OnLevelWon?.Invoke(timeSpent, stars);
    }
}