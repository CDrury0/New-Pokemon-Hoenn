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
        GameObject baseMove = MoveData.GetBaseMove(recordOfUserBeingAttacked.moveUsed);
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(baseMove.GetComponent<MoveData>().targetType, user)){
            user.individualBattleModifier.targets = new List<BattleTarget>(){CombatSystem.BattleTargets.Find(b => b.pokemon == recordOfUserBeingAttacked.user)};
        }
        user.turnAction = baseMove;
        GameObject instantiatedMove = Instantiate(user.turnAction);
        yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, instantiatedMove, true, false));
    }
}
