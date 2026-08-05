using UnityEngine;

[CreateAssetMenu(fileName = "NewTrapCard", menuName = "Zombified/Trap Card Data")]
public class TrapCardData : ScriptableObject
{
    public string trapName;
    public GameObject trapPrefab;
}