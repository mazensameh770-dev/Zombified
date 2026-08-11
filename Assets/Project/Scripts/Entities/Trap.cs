using UnityEngine;

public class Trap : GridObject
{
    [Header("Trap Settings")]
    [SerializeField] private TrapEffectSO trapEffect;
    public override void ObjectPlaced(GridTile tile) {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTile tile) {
        base.ObjectRemoved(tile);
        //CardSelectionManager.Instance.RestoreOne()
    }
    public override void SteppedOn(GridObject soldier) {
        trapEffect.Execute(this, soldier);
    }
    public TrapEffectSO GetTrapEffect() {
        return trapEffect;
    }
}
