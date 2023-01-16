using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAI : EnemyAI
{
    public override void ChooseAction(BattleTarget user)
    {
        if(CombatLib.Instance.moveFunctions.LockedIntoAction(user)){
            return;
        }

        List<GameObject> possibleActions = GetPossibleActions(user);
        user.turnAction = possibleActions[Random.Range(0, possibleActions.Count)];
        
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(user.turnAction.GetComponent<MoveData>().targetType, user)){
            CombatLib.Instance.moveFunctions.MustChooseTarget(TargetType.RandomFoe, user);
        }
    }
}
