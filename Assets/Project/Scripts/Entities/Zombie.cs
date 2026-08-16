using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class Zombie : GridObject
{
    [Header("Zombie Settings")]
    [SerializeField] private Animator animator;
    private GridTile target = null;
    private int health = 4;
    private bool canMove = true;

    protected override void PlayNextAction()
    {
        if (!canMove) return;
        target = null;
        GridObject.StartSearching(currentGridTile, range, tile => {
            GridObject obj = tile.GetCurrentObject();
            if (obj != null && target == null && obj is Soldier)
            {
                target = obj.GetCurrentTile();
            }
        });
        if (target != null)
        {
            GameManager.Instance.CheckSum();
            CheckingDirection();
        }
    }

    protected override void ResetObject()
    {
        transform.DOKill();
        if (currentGridTile != null)
        {
            currentGridTile.RemoveObject();
        }
        else
        {
            Destroy(gameObject);
        }
            
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        canMove = false;
        if (animator != null)
        {
            animator.SetTrigger("die");
        }
    }

    private void CheckingDirection()
    {
        Vector3 direction = (target.transform.position - currentGridTile.transform.position).normalized;

        GridTile targetedTile = null;
        if (Mathf.Abs(direction.z) >= Mathf.Abs(direction.x))
        {
            if (direction.z >= 0)
            {
                targetedTile = currentGridTile.GetFront();
                transform.DORotate(Vector3.zero, 0.25f);
            }
            else
            {
                targetedTile = currentGridTile.GetBack();
                transform.DORotate(new Vector3(0, 180, 0), 0.25f);
            }
        }
        else
        {
            if (direction.x >= 0)
            {
                targetedTile = currentGridTile.GetRight();
                transform.DORotate(new Vector3(0, 90, 0), 0.25f);
            }
            else
            {
                targetedTile = currentGridTile.GetLeft();
                transform.DORotate(new Vector3(0, -90, 0), 0.25f);
            }
        }
        Moving(targetedTile);
    }

    private async void Moving(GridTile targetTile)
    {
        if (animator != null) animator.SetBool("walk", true);
        currentGridTile.MoveObject(targetTile);

        await Task.Delay(500);

        if (this != null && gameObject != null && animator != null)
        {
            animator.SetBool("walk", false);
        }
    }

    public bool IsAlive()
    {
        return canMove;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}