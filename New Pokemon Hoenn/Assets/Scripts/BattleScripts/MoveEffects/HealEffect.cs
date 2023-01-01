using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MoveEffect
{
    public bool healChangesWithWeather;
    public bool swallow;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        float healPercent = 0.5f;
        if(healChangesWithWeather){
            switch(CombatSystem.Weather){
                case Weather.None:
                break;
                case Weather.Sunlight:
                healPercent = 0.66f;
                break;
                default:
                healPercent = 0.33f;
                break;
            }
        }
        if(swallow){
            healPercent = 0.33f * user.individualBattleModifier.stockpileCount;
        }
        int healAmount = (int)(target.pokemon.stats[0] * healPercent);
        yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, healAmount));
        target.pokemon.CurrentHealth += healAmount;
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target)));
    }
}
