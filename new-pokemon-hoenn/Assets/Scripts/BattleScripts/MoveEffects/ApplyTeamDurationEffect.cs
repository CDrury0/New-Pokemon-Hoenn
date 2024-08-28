using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDurationEffect{None, Safeguard, StatChangeImmune, Reflect, LightScreen} 
public class ApplyTeamDurationEffect : MoveEffect, ICheckMoveEffectFail
{
    public TeamDurationEffect durationEffect;
    public Weather weatherSet;
    public int timer;
    [SerializeField] private string endMessage;

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect.durationEffect == durationEffect) != null){
            return true;
        }
        //check for weather immunity (cloud nine / air lock)
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(durationEffect != TeamDurationEffect.None){
            target.teamBattleModifier.teamEffects.Add(new TeamDurationEffectInfo(this, timer));
            string outputMessage = ReplaceBattleMessage(user, target, moveData);
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(outputMessage));
        }

        if(weatherSet != null){
            CombatSystem.Weather = weatherSet;
            CombatSystem.weatherTimer = timer;
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(weatherSet.textOnSet));
        }
    }

    public string GetEndMessage(TeamBattleModifier whichTeam){
        return endMessage.Replace("&userPossessive", whichTeam.teamPossessive);
    }
}
