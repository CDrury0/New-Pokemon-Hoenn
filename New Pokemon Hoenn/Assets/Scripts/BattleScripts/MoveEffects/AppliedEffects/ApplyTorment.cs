using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTorment : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is no longer tormented!"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        effectInfo.timer--;
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        return new List<GameObject>(){MultiTurnData.GetBaseMove(CombatSystem.MoveRecordList.FindLast(record => record.user == target.pokemon)?.moveUsed.GetComponent<MoveData>())};
    }
}