using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAI : EnemyAI
{
    protected override void SetTurnAction(BattleTarget user){
        List<GameObject> possibleActions = GetPossibleMoves(user);
        possibleActions.AddRange(GetUsableItems(user.pokemon));
        user.turnAction = possibleActions[Random.Range(0, possibleActions.Count)];
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(user.turnAction.GetComponent<MoveData>().targetType, user)){
            CombatLib.Instance.moveFunctions.MustChooseTarget(TargetType.RandomFoe, user);
        }
    }

    public override Pokemon SelectNextPokemon(Party enemyParty){
        return enemyParty.GetFirstAvailable();
    }
}
