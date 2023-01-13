using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileEffect : MoveEffect, ICheckMoveFail
{
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(user.individualBattleModifier.stockpileCount == IndividualBattleModifier.MAX_STOCKPILE_COUNT){
            return MoveData.FAIL;
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        user.individualBattleModifier.stockpileCount++;
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(user.GetName() + " stockpiled " + user.individualBattleModifier.stockpileCount + "!"));
    }
}
