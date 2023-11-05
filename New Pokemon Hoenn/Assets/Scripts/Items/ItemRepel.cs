using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepel : ItemEffect
{
    [SerializeField] private int numSteps;
    
    public override bool CanEffectBeUsed(Pokemon p){
        return true;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj){
        Debug.Log("repel used");
        message = itemLogic.itemData.itemName + " will keep weak wild Pokémon away for " + numSteps + " steps";
        yield break;
    }
}
