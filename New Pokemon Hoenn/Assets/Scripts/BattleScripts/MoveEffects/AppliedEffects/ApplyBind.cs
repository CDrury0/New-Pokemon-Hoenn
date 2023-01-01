using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBind : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget user, AppliedEffectInfo effectInfo)
    {
        string moveName = gameObject.GetComponent<MoveData>().moveName;
        if(!effectInfo.inflictor.individualBattleModifier.inflictingEffects.Contains(effectInfo) || effectInfo.timer == 0){
            RemoveEffect(user, effectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(user.GetName() + " was freed from " + moveName + "!"));
            yield break;
        }
        effectInfo.timer--;
        int bindDamage = (int)(0.0625f * user.pokemon.stats[0]);
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon, -bindDamage));
        user.pokemon.CurrentHealth -= bindDamage;
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(user.GetName() + " is hurt by " + moveName + "!"));
    }
}
