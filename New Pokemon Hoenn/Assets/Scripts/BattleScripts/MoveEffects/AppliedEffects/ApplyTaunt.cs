using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTaunt : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        throw new System.NotImplementedException();
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        throw new System.NotImplementedException();
    }
}
