using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyIndividualEffect : MoveEffect, ICheckMoveFail
{
    public float chance;
    public int timerMin;
    public int timerMax;

    public virtual string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(moveData.category == MoveData.Category.Status){
            if(target.individualBattleModifier.appliedIndividualEffects.Find(e => e.effect.GetType() == this.GetType()) != null){
                return target.GetName() + " is already affected by that status effect!";
            }
            else if(ImmuneToEffect(target, moveData)){
                return target.GetName() + " is immune to the status effect!";
            }
        }
        return null;
    }

    public virtual bool ImmuneToEffect(BattleTarget target, MoveData moveData){
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        if(Random.Range(0f, 1f) <= chance && target.individualBattleModifier.appliedIndividualEffects.Find(e => e.effect.GetType() == this.GetType()) == null && !ImmuneToEffect(target, moveData)){
            target.individualBattleModifier.appliedIndividualEffects.Add(new AppliedEffectInfo(this, Random.Range(timerMin, timerMax + 1), user));
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target)));
        }
    }
}
