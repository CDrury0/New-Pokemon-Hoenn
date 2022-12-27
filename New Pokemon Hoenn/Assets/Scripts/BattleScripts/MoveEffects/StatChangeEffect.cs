using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEffect : MoveEffect, ICheckMoveFail
{
    public float chance;
    public int[] statChanges = new int[8];

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(moveData.category == MoveData.Category.Status && chance == 1f && ImmuneToStatChanges(target) && user != target){
            return "It doesn't affect " + target.GetName() + "...";
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(Random.Range(0f, 1f) <= chance && !ImmuneToStatChanges(target)){
            for(int i = 0; i < target.individualBattleModifier.statStages.Length; i++){
                target.individualBattleModifier.statStages[i] += statChanges[i];
                target.individualBattleModifier.CalculateStatMultipliers();

                if(statChanges[i] != 0){
                    string statChangeString = (Mathf.Abs(statChanges[i]) > 1 ? " sharply" : "") + (statChanges[i] > 0 ? " rose" : " fell");
                    yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s " + GetStatName(i) + statChangeString));
                }
            }
        }
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
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect == TeamDurationEffect.StatChangeImmune) != null){
            return true;
        }
        //clear body, etc.
        return false;
    }
}
