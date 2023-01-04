using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDisable : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s " + target.individualBattleModifier.disabledMove.GetComponent<MoveData>().moveName + " is no longer disabled"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        effectInfo.timer--;
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        return new List<GameObject>(){target.individualBattleModifier.disabledMove};
    }
    
    public override void RemoveEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        target.individualBattleModifier.disabledMove = null;
        base.RemoveEffect(target, effectInfo);
    }
}