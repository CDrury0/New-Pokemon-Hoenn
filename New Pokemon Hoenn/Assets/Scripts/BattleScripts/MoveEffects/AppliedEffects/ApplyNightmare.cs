using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyNightmare : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return base.CheckMoveFail(user, target, moveData);
    }
}