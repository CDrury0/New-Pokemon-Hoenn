using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDurationEffect{None, Safeguard, StatChangeImmune, Reflect, LightScreen} 
public class ApplyTeamDurationEffect : MoveEffect, ICheckMoveEffectFail
{
    public TeamDurationEffect durationEffect;
    public Weather weatherSet;
    public int timer;

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect == durationEffect) != null){
            return true;
        }
        //check for weather immunity (cloud nine / air lock)
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        string outputMessage = ReplaceBattleMessage(user, target);
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(outputMessage));

        if(durationEffect != TeamDurationEffect.None){
            target.teamBattleModifier.teamEffects.Add(new TeamDurationEffectInfo(durationEffect, timer));
        }

        if(weatherSet != Weather.None){
            CombatSystem.Weather = weatherSet;
            CombatSystem.weatherTimer = timer;
        }
    }
}
