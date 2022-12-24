using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDurationEffect{None, Safeguard, StatChangeImmune, Reflect, LightScreen} 
public class ApplyTeamDurationEffect : MoveEffect, ICheckMoveFail
{
    public TeamDurationEffect durationEffect;
    public Weather weatherSet;
    public int timer;

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect == durationEffect) != null){
            return "But it failed!";
        }
        //check for weather immunity (cloud nine / air lock)
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(WriteTeamEffectMessage(user, moveData));

        if(durationEffect != TeamDurationEffect.None){
            user.teamBattleModifier.teamEffects.Add(new TeamDurationEffectInfo(durationEffect, timer));
        }

        if(weatherSet != Weather.None){
            CombatSystem.Weather = weatherSet;
            CombatSystem.weatherTimer = timer;
        }
    }

    private IEnumerator WriteTeamEffectMessage(BattleTarget target, MoveData moveData){
        string message = target.teamBattleModifier.teamPossessive + moveData.moveName;
        switch(durationEffect){
            case TeamDurationEffect.Safeguard:
            message += " protects its team with a veil!";
            break;
            case TeamDurationEffect.StatChangeImmune:
            message += " shrouds its team!";
            break;
            case TeamDurationEffect.Reflect:
            message += " raised Defense!";
            break;
            case TeamDurationEffect.LightScreen:
            message += " raised Special Defense!";
            break;
        }
        if(durationEffect != TeamDurationEffect.None){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(message));
        }

        switch(weatherSet){
            case Weather.Rain:
            message = "It started to rain!";
            break;
            case Weather.Sunlight:
            message = "The sunlight became intense!";
            break;
            case Weather.Hail:
            message = "It started to hail!";
            break;
            case Weather.Sandstorm:
            message = "A sandstorm brewed!";
            break;
        }
        if(weatherSet != Weather.None){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(message));
        }
    }
}
