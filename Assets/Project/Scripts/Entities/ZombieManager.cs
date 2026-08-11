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

    public void Zombifying(GridTile tile) {
        tile.RemoveObject();
        GameObject zombie = Instantiate(zombiePrefab);
        tile.PlaceObject(zombie.GetComponent<GridObject>());
    }
}
