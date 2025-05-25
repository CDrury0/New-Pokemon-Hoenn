using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMoveCondition : EventCondition
{
    [SerializeField] private GameObject requiredMove;

    public override bool IsConditionTrue() {
        var fieldMove = requiredMove.GetComponent<FieldMove>();
        if(fieldMove is null || !fieldMove.IsFieldUseEligible())
            return false;

        foreach(Pokemon p in PlayerParty.Instance.playerParty.party){
            if(p.moves.Contains(requiredMove))
                return true;
        }
        return false;
    }
}
