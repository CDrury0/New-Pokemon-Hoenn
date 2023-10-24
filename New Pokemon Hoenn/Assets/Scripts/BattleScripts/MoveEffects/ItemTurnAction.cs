using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTurnAction : MoveEffect
{
    public ItemLogic itemLogic;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        yield return StartCoroutine(itemLogic.DoItemEffects(user.battleHUD, user.pokemon,
        (string message) => { return CombatLib.Instance.WriteGlobalMessage(message); }));
    }
}
