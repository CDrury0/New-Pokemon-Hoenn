using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreaksWalls : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        TeamDurationEffectInfo reflectEffectInfo = target.teamBattleModifier.teamEffects.Find(e => e.effect == TeamDurationEffect.Reflect);
        TeamDurationEffectInfo lightScreenEffectInfo = target.teamBattleModifier.teamEffects.Find(e => e.effect == TeamDurationEffect.LightScreen);

        if(reflectEffectInfo != null){
            target.teamBattleModifier.teamEffects.Remove(reflectEffectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.teamBattleModifier.teamPossessive + " reflect was broken!"));
        }
        if(lightScreenEffectInfo != null){
            target.teamBattleModifier.teamEffects.Remove(lightScreenEffectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.teamBattleModifier.teamPossessive + " light screen was broken!"));
        }
    }
}
