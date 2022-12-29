using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyConfuse : ApplyIndividualEffect
{
    public override bool ImmuneToEffect(BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.IsThisType(StatLib.Type.Bug)){
            return true;
        }
        return false;
    }
}
