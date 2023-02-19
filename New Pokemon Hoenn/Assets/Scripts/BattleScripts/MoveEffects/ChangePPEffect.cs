using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePPEffect : MoveEffect, ICheckMoveFail
{
    public int ppChange;

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(MoveData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon)?.moveUsed) == null){
            return MoveData.FAIL;
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int whichMove = target.pokemon.moves.IndexOf(MoveData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon).moveUsed));
        target.pokemon.movePP[whichMove] += ppChange;
        CorrectPPOverflow(target, whichMove);
        string ppMessage = ppChange > 0 ? " gained PP!" : " lost PP!";
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + ppMessage));
    }

    private void CorrectPPOverflow(BattleTarget target, int whichMove){
        if(target.pokemon.movePP[whichMove] < 0){
            target.pokemon.movePP[whichMove] = 0;
        }
        else if(target.pokemon.movePP[whichMove] > target.pokemon.moveMaxPP[whichMove]){
            target.pokemon.movePP[whichMove] = target.pokemon.moveMaxPP[whichMove];
        }
    }
}
