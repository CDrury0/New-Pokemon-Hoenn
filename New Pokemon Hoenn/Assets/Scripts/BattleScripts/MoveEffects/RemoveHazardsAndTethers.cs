using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveHazardsAndTethers : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        List<AppliedEffectInfo> effectsToRemove = new List<AppliedEffectInfo>();
        AppliedEffectInfo effectInfo = user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyBind);
        if(effectInfo != null){
            effectsToRemove.Add(effectInfo);
        }
        effectInfo = user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyLeechSeed);
        if(effectInfo != null){
            effectsToRemove.Add(effectInfo);
        }
        user.teamBattleModifier.spikesCount = 0;
        Debug.Log("cleared hazards and tether effects");
        yield break;
    }
}
