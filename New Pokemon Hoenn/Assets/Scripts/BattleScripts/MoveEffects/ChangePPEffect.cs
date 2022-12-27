using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePPEffect : MoveEffect
{
    public int ppChange;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int whichMove = target.pokemon.moves.IndexOf(MultiTurnData.GetBaseMove(target.individualBattleModifier.lastUsedMove.GetComponent<MoveData>()));
        target.pokemon.movePP[whichMove] += ppChange;
        string ppMessage = ppChange > 0 ? " gained PP!" : " lost PP!";
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + ppMessage));
    }
}
