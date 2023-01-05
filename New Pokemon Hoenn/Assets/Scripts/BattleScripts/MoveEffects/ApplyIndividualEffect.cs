using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyIndividualEffect : MoveEffect, ICheckMoveEffectFail
{
    public float chance;
    public int timerMin;
    public int timerMax;

    public virtual bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        if(Random.Range(0f, 1f) <= chance && target.individualBattleModifier.appliedEffects.Find(e => e.effect.GetType() == this.GetType()) == null && !ImmuneToEffect(user, target, moveData)){
            AppliedEffectInfo effectInfo = new AppliedEffectInfo(this, Random.Range(timerMin, timerMax + 1), user);
            target.individualBattleModifier.appliedEffects.Add(effectInfo);
            user.individualBattleModifier.inflictingEffects.Add(effectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target)));
        }
    }

    public virtual void RemoveEffect(BattleTarget target, AppliedEffectInfo effectInfo){
        effectInfo.inflictor.individualBattleModifier.inflictingEffects.Remove(effectInfo);
        target.individualBattleModifier.appliedEffects.Remove(effectInfo);
    }

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.individualBattleModifier.appliedEffects.Find(e => e.effect.GetType() == this.GetType()) != null){
            return true;
        }
        if(ImmuneToEffect(user, target, moveData)){
            return true;
        }
        return false;
    }
}
