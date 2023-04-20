using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnitudeDamage : NormalDamage
{
    public PiecewiseDamageData piecewiseDamage;
    private int roll;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        //since a new object is created each time the move is used, roll will be 0 here if it is the first time damage is calculated
        if(roll == 0){
            roll = GetMagnitude(Random.Range(0f, 1f));
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(moveData.moveName + " " + roll));
        }
        int power = piecewiseDamage.GetPower(roll);
        yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(target, moveData.GetEffectiveMoveType(user.pokemon)));
    }

    private int GetMagnitude(float rand){
        switch(rand){
            case float i when i < 0.05:
            return 4;
            case float i when i < 0.15:
            return 5;
            case float i when i < 0.35:
            return 6;
            case float i when i < 0.65:
            return 7;
            case float i when i < 0.85:
            return 8;
            case float i when i < 0.95:
            return 9;
            default:
            return 10;
        }
    }
}
