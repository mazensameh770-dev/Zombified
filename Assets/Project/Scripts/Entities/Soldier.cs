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

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField][Range(0f, 1f)] private float shootVolume = 0.5f;
    [SerializeField] private float shootSoundDuration = 0.3f;

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
        if (myPath == null || myPath.actions == null || currentActionIndex >= myPath.actions.Length) return;

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

        ActionType actionToExecute = myPath.actions[currentActionIndex];
        currentActionIndex++;
        ExecuteAction(actionToExecute);
    }

    protected override void ResetObject()
    {
        if (this == null || gameObject == null) return;

        StopAllCoroutines();
        transform.DOKill();

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        gameObject.SetActive(true);

        currentActionIndex = 0;
        canMove = true;
        distracted = false;

        if (currentGridTile != null)
        {
            currentGridTile.RemoveObject(false);
            currentGridTile = null;
        }

        if (startingTile != null)
        {
            startingTile.RemoveObject(false);
            startingTile.PlaceObject(this);
            transform.position = startingPosition;
        }

        transform.rotation = startingRotation;

        if (animator != null && gameObject.activeInHierarchy)
        {
            animator.ResetTrigger("shoot");
            animator.ResetTrigger("die");
            animator.SetBool("walk", false);
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
        if (targeted == null) return;
        transform.LookAt(targeted.transform.position);
        if (animator != null) animator.SetTrigger("shoot");

        if (audioSource != null && shootSound != null)
        {
            StartCoroutine(PlayShortenedShootSound());
        }

        targeted.GetCurrentObject()?.TakeDamage(1);
    }

    private IEnumerator PlayShortenedShootSound()
    {
        audioSource.clip = shootSound;
        audioSource.volume = shootVolume;
        audioSource.time = 0f;
        audioSource.Play();
        yield return new WaitForSeconds(shootSoundDuration);
        audioSource.Stop();
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
        TryMove(-transform.right, -90f);
    }

    private void MoveRight()
    {
        TryMove(transform.right, 90f);
    }

    private void MoveForward()
    {
        TryMove(transform.forward, 0f);
    }

    private void TryMove(Vector3 dir, float rotateY)
    {
        if (this == null || currentGridTile == null) return;

        GridTile targetTile = null;

        if (Vector3.Dot(dir, Vector3.forward) > 0.7f) targetTile = currentGridTile.GetFront();
        else if (Vector3.Dot(dir, Vector3.back) > 0.7f) targetTile = currentGridTile.GetBack();
        else if (Vector3.Dot(dir, Vector3.left) > 0.7f) targetTile = currentGridTile.GetLeft();
        else if (Vector3.Dot(dir, Vector3.right) > 0.7f) targetTile = currentGridTile.GetRight();

        if (targetTile == null) return;

        GridObject objOnTile = targetTile.GetCurrentObject();
        if (objOnTile is Trap trap && trap.BlocksSoldier)
        {
            trap.TriggerError();
            return;
        }

        if (Mathf.Abs(rotateY) > 0.01f)
        {
            transform.DORotate(transform.eulerAngles + new Vector3(0, rotateY, 0), 0.25f);
        }

        Moving(targetTile);
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
        StopAllCoroutines();
        transform.DOKill();

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (currentGridTile != null)
        {
            currentGridTile.RemoveObject(false);
            currentGridTile = null;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}