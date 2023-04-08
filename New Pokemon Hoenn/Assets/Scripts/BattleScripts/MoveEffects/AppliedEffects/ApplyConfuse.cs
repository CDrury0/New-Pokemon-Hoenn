using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyConfuse : ApplyIndividualEffect
{
    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.IsThisType(Pokemon.Type.Bug)){
            return true;
        }
        return false;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(base.DoEffect(user, target, moveData));
    }

    void Awake(){
        message = "&targetName was confused!";
    }
}
