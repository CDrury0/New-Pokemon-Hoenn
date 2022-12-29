using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDrowsy : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override bool ImmuneToEffect(BattleTarget target, MoveData moveData)
    {
        return base.ImmuneToEffect(target, moveData);
    }
}