using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMoveEffect : CallMoveEffect, ICheckMoveFail
{
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        MoveRecordList.MoveRecord recordOfUserBeingAttacked = CombatSystem.MoveRecordList.FindRecordMirrorMove(user.pokemon, prohibitedMoves);
        if(recordOfUserBeingAttacked != null){
            return null;
        }
        return MoveData.FAIL;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        MoveRecordList.MoveRecord recordOfUserBeingAttacked = CombatSystem.MoveRecordList.FindRecordMirrorMove(user.pokemon, prohibitedMoves);
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(recordOfUserBeingAttacked.moveUsed.GetComponent<MoveData>().targetType, user, CombatLib.Instance.combatSystem.BattleTargets, CombatLib.Instance.combatSystem.DoubleBattle)){
            user.individualBattleModifier.targets = new List<BattleTarget>(){CombatLib.Instance.combatSystem.BattleTargets.Find(b => b.pokemon == recordOfUserBeingAttacked.user)};
        }
        user.turnAction = recordOfUserBeingAttacked.moveUsed;
        GameObject instantiatedMove = Instantiate(user.turnAction);
        yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, instantiatedMove, true, false));
    }
}
