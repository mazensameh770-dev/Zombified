using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public event Action OnNextTurn;
    public event Action OnSimulationEnd;

    //private GridObject[] LevelObjects;
    [SerializeField] private int simulationTurns;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public async void Simulate() {
        //LevelObjects = FindObjectsByType<GridObject>(FindObjectsSortMode.None);
        for (int i = 0; i < simulationTurns; i++) {
            /*
            foreach (var obj in LevelObjects) {
                obj.PlayNextAction();
            }
            */
            OnNextTurn?.Invoke();
            await Task.Delay(1000);
        }
        await Task.Delay(2000);
        OnSimulationEnd?.Invoke();
        //ResetLevel();
    }
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
