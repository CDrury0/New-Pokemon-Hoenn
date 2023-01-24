using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MoveEffect, ICheckMoveEffectFail
{
    public float baseHealPercent;
    public bool healChangesWithWeather;
    public bool swallow;

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(swallow && user.individualBattleModifier.stockpileCount == 0){
            return true;
        }
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        float healPercent = baseHealPercent != 0 ? baseHealPercent : 0.5f;
        if(healChangesWithWeather){
            if(CombatSystem.Weather.healsLess){
                healPercent *= 0.5f;
            }
            if(CombatSystem.Weather.healsMore){
                healPercent *= 1.5f;
            }
        }
        if(swallow){
            healPercent = 0.333f * (float)(user.individualBattleModifier.stockpileCount);
        }
        int healAmount = (int)(target.pokemon.stats[0] * healPercent);
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, healAmount));
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target, moveData)));
    }

    void Awake(){
        message = "&userName restored health!";
    }
}
