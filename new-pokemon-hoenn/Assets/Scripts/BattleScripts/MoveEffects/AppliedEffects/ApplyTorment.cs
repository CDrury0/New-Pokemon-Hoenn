using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTorment : ApplyIndividualEffect, ICheckMoveSelectable
{
    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        return new List<GameObject>() {MoveData.GetBaseMove(CombatSystem.MoveRecordList.FindRecordLastUsedBy(target.pokemon)?.moveUsed)};
    }

    void Awake(){
        message = "&targetName was subjected to torment!";
    }
}