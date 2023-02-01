using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatEffect : MoveEffect
{
    public bool haze;
    public bool[] statsReset = new bool[8];
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(haze){
            for(int i = 0; i < CombatSystem.BattleTargets.Count; i++){
                ResetStats(CombatSystem.BattleTargets[i]);
            }
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage("All stat stages were reset!"));
        }
        else{
            ResetStats(target);
        }
    }

    private void ResetStats(BattleTarget target){
        for(int i = 0; i < statsReset.Length; i++){
            if(statsReset[i]){
                target.individualBattleModifier.statStages[i] = 0;
            }
        }
        target.individualBattleModifier.CalculateStatMultipliers();
    }
}
