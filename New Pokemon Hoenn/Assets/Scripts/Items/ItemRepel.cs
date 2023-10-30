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

    public override IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput){
        Debug.Log("repel used");
        yield return StartCoroutine(messageOutput(itemLogic.itemData.itemName + " will keep weak wild Pokémon away for " + numSteps + " steps"));
    }
}
