using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this as T)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} destroyed on {gameObject.name}.");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
}