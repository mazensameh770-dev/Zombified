using UnityEngine;

public class Soldier : GridObject {

    [SerializeField] private PathSO myPath;
    private int currentActionIndex = 0;
    public override void ObjectPlaced(GridTileState tile) {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTileState tile) {
        base.ObjectRemoved(tile);
    }

    public override void PlayNextAction() {
        if (currentActionIndex >= myPath.actions.Length) return;
        // check for interruptions within range
        ExecuteAction(myPath.actions[currentActionIndex]);
        currentActionIndex++;
    }

    private void ExecuteAction(ActionType action) {
        switch (action) {
            case ActionType.moveForward:
                MoveForward();
                break;
            case ActionType.moveRight:
                MoveRight();
                break;
            case ActionType.moveLeft:
                MoveLeft();
                break;
            case ActionType.rotateBack:
                RotateBack();
                break;
            case ActionType.idle:
                Idle();
                break;
        }
    }

    private void Idle() {

    }

    private void RotateBack() {

    }

    private void MoveLeft() {

    }

    private void MoveRight() {

    }

    private void MoveForward() {

    }
}
