using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class Soldier : GridObject {

    [Header("Soldier Settings")]
    [SerializeField] private PathSO myPath;
    [SerializeField] private Animator animator;
    [SerializeField] private GridTile startingTile;
    private int currentActionIndex = 0;
    private bool canMove = true;

    private void Start() {
        startingTile.PlaceObject(this);
    }
    public override void ObjectPlaced(GridTile tile) {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTile tile) {
        base.ObjectRemoved(tile);
    }

    public override void Die() {
        animator.SetTrigger("die");
        canMove = false;
    }

    public override void PlayNextAction() {
        if (!canMove) return;
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
        transform.DORotate(transform.eulerAngles + new Vector3(0, 180, 0), 0.5f);
    }

    private void MoveLeft() {
        transform.DORotate(transform.eulerAngles + new Vector3(0, -90, 0), 0.25f);
        CheckDirection(-transform.right);
    }

    private void MoveRight() {
        transform.DORotate(transform.eulerAngles + new Vector3(0, 90, 0), 0.25f);
        CheckDirection(transform.right);
    }

    private void MoveForward() {
        CheckDirection(transform.forward);
    }
    private void CheckDirection(Vector3 direction) {
        if (direction == Vector3.forward) {
            Moving(currentGridTile.GetFront());
        } else if (direction == Vector3.back) {
            Moving(currentGridTile.GetBack());
        } else if (direction == Vector3.left) {
            Moving(currentGridTile.GetLeft());
        } else if (direction == Vector3.right) {
            Moving(currentGridTile.GetRight());
        }
    }
    private async void Moving(GridTile targetTile) {
        animator.SetBool("walk", true);
        currentGridTile.MoveObject(targetTile);
        await Task.Delay(500);
        animator.SetBool("walk", false);
    }
    public void ResetSoldier() {
        print("Resetting");
        currentActionIndex = 0;
        currentGridTile?.RemoveObject(false);
        startingTile.PlaceObject(this);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (!canMove) animator.SetTrigger("reset");
        canMove = true;
    }
}
