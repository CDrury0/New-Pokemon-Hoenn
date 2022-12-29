using System.Collections.Generic;
using UnityEngine;

public class ApplyTorment : ApplyIndividualEffect, ICheckMoveSelectable
{
    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        throw new System.NotImplementedException();
    }
}