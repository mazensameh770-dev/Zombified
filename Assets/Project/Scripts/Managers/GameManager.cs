using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GridObject[] LevelObjects;
    [SerializeField] private int simulationTurns;

    public async void Simulate() {
        LevelObjects = FindObjectsByType<GridObject>(FindObjectsSortMode.None);
        for (int i = 0; i < simulationTurns; i++) {
            foreach (var obj in LevelObjects) {
                obj.PlayNextAction();
            }
            await Task.Delay(1000);
        }
        await Task.Delay(2000);
        ResetLevel();
    }
    private void ResetLevel() {
        print("Game over, resetting");
        foreach (var obj in LevelObjects) {
            obj.gameObject.SetActive(true);
            if (obj is Soldier) {
                (obj as Soldier).ResetSoldier();
            }
        }
    }
}
