using UnityEngine;

[CreateAssetMenu(fileName = "New Path", menuName = "Soldier/PathSO")]
public class PathSO : ScriptableObject
{
    public int actionLength;
    public ActionType[] actions;


    [ContextMenu("Set Action Length")]
    private void SetActionLength() {
        actions = new ActionType[actionLength];
    }
}

public enum ActionType {
    moveForward,
    moveRight,
    moveLeft,
    rotateBack,
    idle
}
