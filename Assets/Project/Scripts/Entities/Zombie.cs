using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Zombie : GridObject
{
    [Header("Zombie Settings")]
    [SerializeField] private Animator animator;
    private GridTile target = null;
    [SerializeField] private int maxHealth = 4;
    private int currentHealth;
    private bool canMove = true;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public event System.Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

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
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
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
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        canMove = false;
        if (currentGridTile != null)
        {
            currentGridTile.RemoveObject(false);
        }
        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        Destroy(gameObject, 2f);
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

    private void Moving(GridTile targetTile)
    {
        StartCoroutine(MovingRoutine(targetTile));
    }

    private IEnumerator MovingRoutine(GridTile targetTile)
    {
        if (animator != null) animator.SetBool("walk", true);
        currentGridTile.MoveObject(targetTile);

        yield return new WaitForSeconds(0.5f);

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