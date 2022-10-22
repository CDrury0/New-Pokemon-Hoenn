using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TetherEffect {Bind, StopEscape, Infatuated};
public class ApplyTetherEffect : ApplyEffect
{
    public TetherEffect tetherEffect;
    public override IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
