using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyConfuse : ApplyIndividualEffect
{
    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.IsThisType(StatLib.Type.Bug)){
            return true;
        }
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        message = "&targetName was confused!";
        yield return StartCoroutine(base.DoEffect(user, target, moveData));
    }
}
