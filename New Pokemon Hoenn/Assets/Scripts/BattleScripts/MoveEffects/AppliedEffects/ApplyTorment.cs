using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTorment : ApplyIndividualEffect, ICheckMoveSelectable
{
    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        return new List<GameObject>(){MultiTurnData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon)?.moveUsed.GetComponent<MoveData>())};
    }
}