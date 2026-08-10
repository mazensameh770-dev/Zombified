using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "trapEffect", menuName = "Trap/Trap_Effect")]
public class TrapEffectSO : ScriptableObject {
    [SerializeReference] public List<TrapEffect> effects;
    private void OnEnable() {
        if (effects == null) effects = new List<TrapEffect>();
    }
    public void Execute(Trap trap) {
        foreach (TrapEffect effect in effects) {
            if (effect == null) continue;
            effect.Execute(trap);
        }
    }
}

[Serializable]
public abstract class TrapEffect {
    public virtual void Execute(Trap trap) { }
    public virtual void ChainedEffect(Trap trap) { }
}

[Serializable]
public class Explode : TrapEffect {

    public override void Execute(Trap tap) {
        // explode
    }

    public override void ChainedEffect(Trap trap) {
        // explode
    }
}

[Serializable]
public class ExplosiveOnImpact : TrapEffect {

    public override void ChainedEffect(Trap trap) {
        // explode
    }
}

[Serializable]
public class Lure : TrapEffect {
    public bool LureSoldiers = true;
    public override void Execute(Trap tap) {
        if (LureSoldiers) {
            // lure soldiers
        } else {
            // lure 
        }
    }
}

[Serializable]
public class Zombify : TrapEffect {
    public override void Execute(Trap tap) {
        // Zombify
    }
}