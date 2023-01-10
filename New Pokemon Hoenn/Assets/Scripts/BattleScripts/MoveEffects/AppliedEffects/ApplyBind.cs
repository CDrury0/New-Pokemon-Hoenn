using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBind : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        string moveName = gameObject.GetComponent<MoveData>().moveName;
        if(!effectInfo.inflictor.individualBattleModifier.inflictingEffects.Contains(effectInfo) || effectInfo.timer == 0){
            RemoveEffect(target, effectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " was freed from " + moveName + "!"));
            yield break;
        }
        effectInfo.timer--;
        int bindDamage = (int)(0.0625f * target.pokemon.stats[0]);
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -bindDamage));
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is hurt by " + moveName + "!"));
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        message = "&targetName is trapped by &userName's &moveName!";
        yield return StartCoroutine(base.DoEffect(user, target, moveData));
    }
}
