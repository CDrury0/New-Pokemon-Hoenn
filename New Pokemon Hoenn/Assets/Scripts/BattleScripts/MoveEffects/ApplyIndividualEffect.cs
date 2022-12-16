using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyIndividualEffect : MoveEffect, IApplyEffect
{
    public bool applyToSelf;
    public int timer;

    public abstract IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData);

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
