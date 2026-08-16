using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "trapEffect", menuName = "Trap/Trap_Effect")]
public class TrapEffectSO : ScriptableObject {
    [SerializeReference] public List<TrapEffect> effects;
    private void OnEnable() {
        if (effects == null) effects = new List<TrapEffect>();
    }
    public void Execute(Trap trap, GridObject obj) {
        foreach (TrapEffect effect in effects) {
            if (effect == null) continue;
            effect.Execute(trap, obj);
        }
    }
    public void ExecuteChained(Trap trap) {
        foreach (TrapEffect effect in effects) {
            if (effect == null) continue;
            effect.ChainedEffect(trap);
        }
    }
}

[Serializable]
public abstract class TrapEffect {
    public virtual void Execute(Trap trap, GridObject obj) { }
    public virtual void ChainedEffect(Trap trap) { }
    protected void ApplyExplosion(Trap trap) {
        GridTile currentTile = trap.GetCurrentTile();
        if (ParticlesManager.Instance != null)
            ParticlesManager.Instance.SpawnExplosion(currentTile);
        GridObject.StartSearching(currentTile, trap.getRange(), (tile) => {
            GridObject obj = tile.GetCurrentObject();
            //Debug.Log($"Checking tile {tile.gameObject.name}");
            if (obj == null) return;
            if (obj is Trap) {
                if (!obj.gameObject.activeInHierarchy) return;
                //Debug.Log("Found Trap");
                ((Trap)obj).GetTrapEffect().ExecuteChained((Trap)obj);
            }
            if ((obj is Soldier) || (obj is Zombie)) {
                //Debug.Log("Found soldier");
                obj.TakeDamage(5);
            }
        });
    }
}

[Serializable]
public class Explode : TrapEffect {

    public override void Execute(Trap trap, GridObject obj) {
        obj.TakeDamage(5);
        trap.gameObject.SetActive(false);
        ApplyExplosion(trap);
    }

    public override void ChainedEffect(Trap trap) {
        trap.gameObject.SetActive(false);
        ApplyExplosion(trap);
    }
}

[Serializable]
public class ExplosiveOnImpact : TrapEffect {

    public override void ChainedEffect(Trap trap) {
        trap.gameObject.SetActive(false);
        ApplyExplosion(trap);
    }
}

[Serializable]
public class Lure : TrapEffect {
    public bool LureSoldiers = true;
    public override void Execute(Trap trap, GridObject obj) {
        if (LureSoldiers) {
            // lure soldiers
        } else {
            // lure 
        }
    }
}

[Serializable]
public class Zombify : TrapEffect {
    public override void Execute(Trap trap, GridObject obj) {
        ZombieManager.Instance.Zombifying(trap.GetCurrentTile(), obj);
    }
}