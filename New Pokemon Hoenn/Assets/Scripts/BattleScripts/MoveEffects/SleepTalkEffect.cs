using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTalkEffect : CallMoveEffect, ICheckMoveFail
{
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        List<GameObject> usableMoves = GetUsableMoves(user);
        if(usableMoves.Count > 0){
            return null;
        }
        return MoveData.FAIL;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        List<GameObject> usableMoves = GetUsableMoves(user);
        GameObject selectedMove = Instantiate(usableMoves[Random.Range(0, usableMoves.Count)]);
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(selectedMove.GetComponent<MoveData>().targetType, user, CombatLib.Instance.combatSystem.BattleTargets, CombatLib.Instance.combatSystem.DoubleBattle)){
            CombatLib.Instance.moveFunctions.MustChooseTarget(TargetType.RandomFoe, user, CombatLib.Instance.combatSystem.BattleTargets, CombatLib.Instance.combatSystem.DoubleBattle);
        }
        yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, selectedMove, true, true));
    }

    private List<GameObject> GetUsableMoves(BattleTarget user){
        List<GameObject> usableMoves = new List<GameObject>(user.pokemon.moves);
        RemoveIllegalMoves(usableMoves);
        usableMoves.RemoveAll(move => user.pokemon.movePP[usableMoves.IndexOf(move)] == 0); //sleep talk considers PP
        return usableMoves;
    }
}
