using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistEffect : CallMoveEffect, ICheckMoveFail
{
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData) {
        if(GetUsableMoves(user, CombatLib.CombatSystem.GetTeamParty(user).members).Count > 0)
            return null;
        
        return MoveData.FAIL;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData) {
        List<GameObject> usableMoves = GetUsableMoves(user, CombatLib.CombatSystem.GetTeamParty(user).members);
        GameObject selectedMove = Instantiate(usableMoves[Random.Range(0, usableMoves.Count)]);
        if(CombatLib.MoveFunctions.MustChooseTarget(selectedMove.GetComponent<MoveData>().targetType, user))
            CombatLib.MoveFunctions.MustChooseTarget(TargetType.RandomFoe, user);

        yield return StartCoroutine(CombatLib.CombatSystem.UseMove(user, selectedMove, true, false));
    }

    private List<GameObject> GetUsableMoves(BattleTarget user, List<Pokemon> allies){
        List<GameObject> usableMoves = new();
        foreach(var ally in allies){
            if(ally != user.pokemon)
                usableMoves.AddRange(ally.moves);
        }
        RemoveIllegalMoves(usableMoves);
        return usableMoves;
    }
}
