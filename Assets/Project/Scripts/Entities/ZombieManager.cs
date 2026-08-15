using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }
    [SerializeField] private GameObject zombiePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Zombifying(GridTile tile, GridObject soldier, GridTile sourceTile = null)
    {
        if (tile == null) return;

        if (soldier is Soldier s)
        {
            s.TurnToZombie();
        }

        if (zombiePrefab == null) return;

        Transform parentTransform = null;
        if (GameManager.Instance != null && GameManager.Instance.CurrentLevelRoot != null)
        {
            parentTransform = GameManager.Instance.CurrentLevelRoot;
        }

        GameObject zombieObj = Instantiate(zombiePrefab, parentTransform);
        zombieObj.transform.position = tile.transform.position;
        zombieObj.transform.rotation = Quaternion.identity;

        GridObject zombieComp = zombieObj.GetComponent<GridObject>();
        if (zombieComp != null)
        {
            tile.PlaceObject(zombieComp);
        }
    }

    public void SpawnZombieOnTrap(GridTile tile)
    {
        if (tile == null) return;

        if (tile.GetCurrentObject() is Trap trap)
        {
            trap.DeactivateTrap();
        }

        if (zombiePrefab == null) return;

        Transform parentTransform = null;
        if (GameManager.Instance != null && GameManager.Instance.CurrentLevelRoot != null)
        {
            parentTransform = GameManager.Instance.CurrentLevelRoot;
        }

        GameObject zombieObj = Instantiate(zombiePrefab, parentTransform);
        zombieObj.transform.position = tile.transform.position;
        zombieObj.transform.rotation = Quaternion.identity;

        GridObject zombieComp = zombieObj.GetComponent<GridObject>();
        if (zombieComp != null)
        {
            tile.PlaceObject(zombieComp);
        }
    }
}