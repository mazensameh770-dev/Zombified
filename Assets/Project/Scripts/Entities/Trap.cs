using UnityEngine;

public class Trap : GridObject
{
    [Header("Trap Settings")]
    [SerializeField] private TrapEffectSO trapEffect;

    [Header("Soldier Block Settings")]
    [SerializeField] private bool blocksSoldier = false;
    [SerializeField] private string soldierWarningMessage = "مينفعش تحط البرميل قدام الجندي!";

    private TrapCardData sourceCardData;
    private GridTile placedTile;

    public TrapCardData TrapData => sourceCardData;
    public bool BlocksSoldier => blocksSoldier;
    public string SoldierWarningMessage => soldierWarningMessage;

    public void SetSourceCardData(TrapCardData cardData)
    {
        sourceCardData = cardData;
    }

    public override void ObjectPlaced(GridTile tile)
    {
        base.ObjectPlaced(tile);
        placedTile = tile;
    }

    public override void ObjectRemoved(GridTile tile)
    {
        base.ObjectRemoved(tile);
        if (GameManager.Instance != null && !GameManager.Instance.IsSimulating)
        {
            if (CardSelectionManager.Instance != null && sourceCardData != null)
            {
                CardSelectionManager.Instance.RestoreOne(sourceCardData);
            }
        }
    }

    public override void SteppedOn(GridObject stepper)
    {
        if (stepper is Soldier && blocksSoldier)
        {
            TriggerError();
            return;
        }

        if (trapEffect != null)
        {
            trapEffect.Execute(this, stepper);
        }

        DeactivateTrap();
    }

    public void DeactivateTrap()
    {
        if (currentGridTile != null)
        {
            GridTile tile = currentGridTile;
            currentGridTile = null;
            if (tile.GetCurrentObject() == (GridObject)this)
            {
                tile.RemoveObject(false);
            }
        }
        gameObject.SetActive(false);
    }

    public void TriggerError()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopSimulationAndReset(false);
        }

        if (NotificationUI.Instance != null)
        {
            NotificationUI.Instance.ShowMessage(soldierWarningMessage);
        }
    }

    protected override void ResetObject()
    {
        gameObject.SetActive(true);
        if (placedTile != null)
        {
            placedTile.PlaceObject(this);
        }
    }

    public TrapEffectSO GetTrapEffect()
    {
        return trapEffect;
    }
}