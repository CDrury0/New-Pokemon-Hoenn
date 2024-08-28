using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTaunt : ApplyIndividualEffect, ICheckMoveSelectable, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " is no longer taunted!"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        effectInfo.timer--;
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        List<GameObject> unusableMoves = new List<GameObject>();
        foreach(GameObject move in target.pokemon.moves){
            if(move.GetComponent<MoveData>().category == MoveData.Category.Status){
                unusableMoves.Add(move);
            }
        }
        return unusableMoves;
    }

    void Awake(){
        message = "&targetName fell for the taunt!";
    }
}
