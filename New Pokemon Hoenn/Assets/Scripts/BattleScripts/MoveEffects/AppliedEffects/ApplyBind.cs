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
        yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, -bindDamage));
        target.pokemon.CurrentHealth -= bindDamage;
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is hurt by " + moveName + "!"));
    }
}
