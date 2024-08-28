using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychUpEffect : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        for(int i = 0; i < target.individualBattleModifier.statStages.Length; i++){
            user.individualBattleModifier.statStages[i] = target.individualBattleModifier.statStages[i];
        }
        user.individualBattleModifier.CalculateStatMultipliers();
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(user.GetName() + " copied " + target.GetName() + "'s stat changes!"));
    }
}
