using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance { get; private set; }
    [SerializeField] private GameObject ExplosionParticle;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SpawnExplosion(GridTile tile) {
        Instantiate(ExplosionParticle, tile.transform.position, ExplosionParticle.transform.rotation);
    }
}
