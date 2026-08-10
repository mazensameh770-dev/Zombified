using UnityEngine;

public class Trap : GridObject
{
    [SerializeField] private TrapEffectSO trapEffect;
    public override void ObjectPlaced(GridTileState tile) {
        base.ObjectPlaced(tile);
    }

    public override void ObjectRemoved(GridTileState tile) {
        base.ObjectRemoved(tile);
        //CardSelectionManager.Instance.RestoreOne()
    }
    public override void SteppedOn() {
        trapEffect.Execute(this);
    }
}
