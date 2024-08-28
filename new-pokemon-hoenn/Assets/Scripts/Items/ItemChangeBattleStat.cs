using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeBattleStat : ItemEffect
{
    [Tooltip("0 is attack")] [SerializeField] private int whichStat;
    [SerializeField] private int numStages;

    public override bool CanEffectBeUsed(Pokemon p) {
        BattleTarget target = CombatSystem.GetBattleTarget(p);
        return p.inBattle
        && !GetStatIsMaxed(target.individualBattleModifier.statStages)
        && !target.teamBattleModifier.usedXItemThisBattle;
    }

    private bool GetStatIsMaxed(int[] stages) {
        if(whichStat <= 4){
            return stages[whichStat] == IndividualBattleModifier.MAX_STAT_STAGES;
        }
        else if(whichStat <= 6){
            return stages[whichStat] == IndividualBattleModifier.MAX_ACCURACY_STAGES;
        }
        else if(whichStat <= 7){
            return stages[whichStat] == IndividualBattleModifier.MAX_CRIT_STAGES;
        }
        else{
            Debug.Log("error checking max stat on X item usage");
            return true;
        }
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput, int whichMove) {
        BattleTarget target = CombatSystem.GetBattleTarget(p);
        int currentStage = target.individualBattleModifier.statStages[whichStat];
        target.individualBattleModifier.statStages[whichStat] = Mathf.Min(currentStage + numStages, IndividualBattleModifier.MAX_STAT_STAGES);
        target.teamBattleModifier.usedXItemThisBattle = true;
        message = p.nickName + "'s " + StatChangeEffect.GetStatName(whichStat) + " sharply rose";
        yield break;
    }
}
