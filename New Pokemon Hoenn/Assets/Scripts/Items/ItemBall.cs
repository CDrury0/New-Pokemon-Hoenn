using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBall : ItemEffect
{
    public override bool CanEffectBeUsed(Pokemon p){
        return CombatSystem.EnemyTrainer == null;
    }

    //this implementation assumes that only the player can use pokeballs
    public override IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput){
        string message = "Player threw a(n) " + itemLogic.itemData.itemName;
        yield return StartCoroutine(messageOutput(message));
    }
}
