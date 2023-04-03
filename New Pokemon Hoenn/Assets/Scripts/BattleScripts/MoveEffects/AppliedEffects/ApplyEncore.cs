using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEncore : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s encore ended"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        effectInfo.timer--;
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon) == null){
            return true;
        }
        return false;
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        List<GameObject> unusableMoves = new List<GameObject>(target.pokemon.moves);
        unusableMoves.Remove(MoveData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon).moveUsed));
        return unusableMoves;
    }

    void Awake(){
        message = "&targetName got an encore!";
    }
}
