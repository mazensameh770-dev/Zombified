using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class Soldier : GridObject {

    private enum SoldierDirection {
        Front,
        Right,
        Left,
        Back
    }


    [Header("Soldier Settings")]
    [SerializeField] private SoldierDirection direction;
    [SerializeField] private PathSO myPath;
    [SerializeField] private Animator animator;
    [SerializeField] private GridTile startingTile;
    private int currentActionIndex = 0;
    private bool canMove = true;
    private bool distracted = false;

    protected override void Start() {
        base.Start();
        startingTile.PlaceObject(this);
        switch (direction) {
            case SoldierDirection.Front:
                transform.forward = Vector3.forward;
                break;
            case SoldierDirection.Right:
                transform.forward = Vector3.right;
                break;
            case SoldierDirection.Left:
                transform.forward = Vector3.left;
                break;
            case SoldierDirection.Back:
                transform.forward = Vector3.back;
                break;
        }
    }
    public override void ObjectPlaced(GridTile tile) {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTile tile) {
        base.ObjectRemoved(tile);
    }

    public override void TakeDamage(int damage) {
        animator.SetTrigger("die");
        canMove = false;
    }

    protected override void PlayNextAction() {
        if (!canMove) return;
        if (currentActionIndex >= myPath.actions.Length) return;
        // check for interruptions within range
        distracted = false;
        GridObject.StartSearching(currentGridTile, range, tile => {
            if (distracted) return;
            GridObject obj = tile.GetCurrentObject();
            if (obj is Zombie && ((Zombie)obj).IsAlive()) {
                ShootZombie(tile);
                distracted = true;
            }
        });
        //print(distracted);
        if (distracted) return;
        // if it is safe. do your action
        ExecuteAction(myPath.actions[currentActionIndex]);
        currentActionIndex++;
    }
    protected override void ResetObject() {
        currentActionIndex = 0;
        gameObject.SetActive(true);
        currentGridTile?.RemoveObject(false);
        startingTile.PlaceObject(this);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (!canMove) animator.SetTrigger("reset");
        canMove = true;
    }

    public bool IsAlive()
    {
        return canMove;
    }

    private void ShootZombie(GridTile targeted) {
        transform.LookAt(targeted.transform.position);
        animator.SetTrigger("shoot");
        targeted.GetCurrentObject().TakeDamage(1);
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

    public void TurnToZombie() {
        canMove = false;
        gameObject.SetActive(false);
    }
}
