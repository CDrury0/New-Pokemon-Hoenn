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
    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput){
        message = "Player threw a(n) " + itemLogic.itemData.itemName;
        yield break;
    }
}
