using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }
    [SerializeField] private GameObject zombiePrefab;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void Zombifying(GridTile tile, GridObject soldier) {
        if (soldier is Soldier) {
            tile.GetCurrentObject().gameObject.SetActive(false);
            tile.RemoveObject(false);
            GameObject zombie = Instantiate(zombiePrefab);
            tile.PlaceObject(zombie.GetComponent<GridObject>());
            ((Soldier)soldier).TurnToZombie();
        }
    }
}
