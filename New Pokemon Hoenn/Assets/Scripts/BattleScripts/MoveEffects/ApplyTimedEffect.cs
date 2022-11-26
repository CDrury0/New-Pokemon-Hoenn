using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTimedEffect : ApplyEffect
{
    public MoveEffect latentEffect;

    public override IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
