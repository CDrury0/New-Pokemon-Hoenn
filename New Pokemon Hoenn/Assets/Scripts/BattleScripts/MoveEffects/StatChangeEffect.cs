using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEffect : MoveEffect, ICheckMoveEffectFail
{
    public bool curse;
    public bool stockpileRemoval;
    public float chance;
    public int[] statChanges = new int[8];

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(curse && user.pokemon.IsThisType(Pokemon.Type.Ghost)){
            return true;
        }
        if(chance == 1f && ImmuneToStatChanges(target) && user != target){
            return true;
        }
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(stockpileRemoval){
            for(int i = 0; i < statChanges.Length; i++){
                statChanges[i] *= user.individualBattleModifier.stockpileCount;
            }
        }
        if(Random.Range(0f, 1f) <= chance && !ImmuneToStatChanges(target)){
            for(int i = 0; i < target.individualBattleModifier.statStages.Length; i++){
                if(StatChangeOutOfBounds(i, target)){
                    string statChangeString = statChanges[i] > 0 ? " won't go higher!" : " won't go lower!";
                    yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + "'s " + GetStatName(i) + statChangeString));
                    continue;
                }

                int max = GetStatMax(i);
                target.individualBattleModifier.statStages[i] += statChanges[i];
                if(target.individualBattleModifier.statStages[i] > max){
                    target.individualBattleModifier.statStages[i] = max;
                }
                target.individualBattleModifier.CalculateStatMultipliers();

                if(statChanges[i] != 0){
                    string statChangeString = (Mathf.Abs(statChanges[i]) > 1 ? " sharply" : "") + (statChanges[i] > 0 ? " rose" : " fell");
                    yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + "'s " + GetStatName(i) + statChangeString));
                }
            }
        }
    }

    private int GetStatMax(int stat){
        if(stat <= 4){
            return IndividualBattleModifier.MAX_STAT_STAGES;
        }
        else if(stat <= 6){
            return IndividualBattleModifier.MAX_ACCURACY_STAGES;
        }
        return IndividualBattleModifier.MAX_CRIT_STAGES;
    }

    private bool StatChangeOutOfBounds(int i, BattleTarget target){
        if(target.individualBattleModifier.statStages[i] == GetStatMax(i) && statChanges[i] > 0){
            return true;
        }
        if(target.individualBattleModifier.statStages[i] == -GetStatMax(i) && statChanges[i] < 0){
            return true;
        }
        return false;
    }

    private string GetStatName(int statNum){
        switch(statNum){
            case 0:
            return "Attack";
            case 1:
            return "Defense";
            case 2:
            return "Special Attack";
            case 3:
            return "Special Defense";
            case 4:
            return "Speed";
            case 5:
            return "Accuracy";
            case 6:
            return "Evasion";
            case 7:
            return "Crit Chance";
            default:
            return "stat change bugged";
        }
    }

    private bool ImmuneToStatChanges(BattleTarget target){
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect.durationEffect == TeamDurationEffect.StatChangeImmune) != null){
            return true;
        }
        //clear body, etc.
        return false;
    }
}
