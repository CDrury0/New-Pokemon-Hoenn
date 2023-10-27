using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTurnAction : MoveEffect
{
    public ItemLogic itemLogic;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        string message = CombatSystem.EnemyTrainer.GetName() + " used " + itemLogic.itemData.itemName + " on " + user.GetName();
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(message));
        yield return StartCoroutine(itemLogic.DoItemEffects(user.battleHUD, user.pokemon, null));
    }
}
