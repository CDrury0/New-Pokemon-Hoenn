using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTimedEffect : MoveEffect, IApplyEffect, ICheckMoveFail
{
    public bool exclusiveWithFutureSight;
    public int timer;
    public GameObject moveUsed;

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        TimedEffectInfo existingEffectInfo = user.individualBattleModifier.timedEffects.Find(e => e.timedEffect.exclusiveWithFutureSight);
        if(exclusiveWithFutureSight && existingEffectInfo != null && existingEffectInfo.target == target){
            return MoveData.FAIL;
        }
        return null;
    }

    public IEnumerator DoAppliedEffect(BattleTarget user, AppliedEffectInfo effectInfo)
    {
        TimedEffectInfo thisInfo = user.individualBattleModifier.timedEffects.Find(e => e.timedEffect == this);
        thisInfo.timer--;
        if(thisInfo.timer == 0){
            user.individualBattleModifier.targets = new List<BattleTarget>(){thisInfo.target};
            CombatLib.Instance.combatSystem.VerifyMoveTarget(user, moveUsed);
            GameObject action = Instantiate(moveUsed);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, action, true, false));
            user.individualBattleModifier.timedEffects.Remove(thisInfo);
        }
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        user.individualBattleModifier.timedEffects.Add(new TimedEffectInfo(this, timer, target));
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target, moveData)));
    }
}
