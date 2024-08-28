using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDisable : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        target.individualBattleModifier.disabledMove = MoveData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon).moveUsed);
        message = target.GetName() + " " + target.individualBattleModifier.disabledMove.GetComponent<MoveData>().moveName + " was disabled!";
        yield return StartCoroutine(base.DoEffect(user, target, moveData));
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        if(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon) == null){
            return true;
        }
        return false;
    }
    
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + "'s " + target.individualBattleModifier.disabledMove.GetComponent<MoveData>().moveName + " is no longer disabled"));
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