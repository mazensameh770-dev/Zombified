using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Soldier : GridObject
{

    private enum SoldierDirection
    {
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

    private Quaternion startingRotation;
    private Vector3 startingPosition;

    protected override void Start()
    {
        base.Start();

        if (startingTile != null)
        {
            startingTile.PlaceObject(this);
            startingPosition = transform.position;
        }

        switch (direction)
        {
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

        startingRotation = transform.rotation;
    }

    public override void ObjectPlaced(GridTile tile)
    {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTile tile)
    {
        base.ObjectRemoved(tile);
    }

    public override void TakeDamage(int damage)
    {
        if (animator != null) animator.SetTrigger("die");
        canMove = false;
    }

    protected override void PlayNextAction()
    {
        if (!canMove) return;
        if (currentActionIndex >= myPath.actions.Length) return;

        distracted = false;
        GridObject.StartSearching(currentGridTile, range, tile => {
            if (distracted) return;
            GridObject obj = tile.GetCurrentObject();
            if (obj is Zombie zombie && zombie.IsAlive())
            {
                ShootZombie(tile);
                distracted = true;
            }
        });

        if (distracted) return;

        ExecuteAction(myPath.actions[currentActionIndex]);
        currentActionIndex++;
    }

    protected override void ResetObject()
    {
        transform.DOKill();
        StopAllCoroutines();

        currentActionIndex = 0;
        canMove = true;
        distracted = false;

        gameObject.SetActive(true);

        if (currentGridTile != null)
        {
            currentGridTile.RemoveObject(false);
        }

        if (startingTile != null)
        {
            startingTile.PlaceObject(this);
            transform.position = startingPosition;
        }

        transform.rotation = startingRotation;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    public bool IsAlive()
    {
        return canMove;
    }

    private void ShootZombie(GridTile targeted)
    {
        transform.LookAt(targeted.transform.position);
        if (animator != null) animator.SetTrigger("shoot");
        targeted.GetCurrentObject()?.TakeDamage(1);
    }

    private void ExecuteAction(ActionType action)
    {
        switch (action)
        {
            case ActionType.moveForward: MoveForward(); break;
            case ActionType.moveRight: MoveRight(); break;
            case ActionType.moveLeft: MoveLeft(); break;
            case ActionType.rotateBack: RotateBack(); break;
            case ActionType.idle: Idle(); break;
        }
    }

    private void Idle() { }

    private void RotateBack()
    {
        transform.DORotate(transform.eulerAngles + new Vector3(0, 180, 0), 0.5f);
    }

    private void MoveLeft()
    {
        transform.DORotate(transform.eulerAngles + new Vector3(0, -90, 0), 0.25f);
        CheckDirection(-transform.right);
    }

    private void MoveRight()
    {
        transform.DORotate(transform.eulerAngles + new Vector3(0, 90, 0), 0.25f);
        CheckDirection(transform.right);
    }

    private void MoveForward()
    {
        CheckDirection(transform.forward);
    }

    private void CheckDirection(Vector3 dir)
    {
        if (dir == Vector3.forward) Moving(currentGridTile.GetFront());
        else if (dir == Vector3.back) Moving(currentGridTile.GetBack());
        else if (dir == Vector3.left) Moving(currentGridTile.GetLeft());
        else if (dir == Vector3.right) Moving(currentGridTile.GetRight());
    }

    private void Moving(GridTile targetTile)
    {
        if (targetTile == null) return;
        StartCoroutine(MoveRoutine(targetTile));
    }

    private IEnumerator MoveRoutine(GridTile targetTile)
    {
        if (animator != null) animator.SetBool("walk", true);
        currentGridTile.MoveObject(targetTile);
        yield return new WaitForSeconds(0.5f);
        if (animator != null) animator.SetBool("walk", false);
    }

    public void TurnToZombie()
    {
        canMove = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}