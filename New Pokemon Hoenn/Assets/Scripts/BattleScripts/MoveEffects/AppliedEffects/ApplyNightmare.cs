using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyNightmare : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget user, AppliedEffectInfo effectInfo)
    {
        throw new System.NotImplementedException();
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return base.ImmuneToEffect(user, target, moveData);
    }
}