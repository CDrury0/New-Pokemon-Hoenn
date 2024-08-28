using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistEffect : CallMoveEffect, ICheckMoveFail
{
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(GetUsableMoves(user, CombatLib.Instance.combatSystem.GetTeamParty(user).party).Count > 0){
            return null;
        }
        return MoveData.FAIL;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        List<GameObject> usableMoves = GetUsableMoves(user, CombatLib.Instance.combatSystem.GetTeamParty(user).party);
        GameObject selectedMove = Instantiate(usableMoves[Random.Range(0, usableMoves.Count)]);
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(selectedMove.GetComponent<MoveData>().targetType, user)){
            CombatLib.Instance.moveFunctions.MustChooseTarget(TargetType.RandomFoe, user);
        }
        yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, selectedMove, true, false));
    }

    private List<GameObject> GetUsableMoves(BattleTarget user, Pokemon[] allies){
        List<GameObject> usableMoves = new List<GameObject>();
        for(int i = 0; i < allies.Length; i++){
            if(allies[i] != null && allies[i] != user.pokemon){
                usableMoves.AddRange(allies[i].moves);
            }
        }
        RemoveIllegalMoves(usableMoves);
        return usableMoves;
    }
}
