using System.Collections.Generic;
using UnityEngine;

public class ApplyDisable : ApplyIndividualEffect, ICheckMoveSelectable
{
    public List<GameObject> GetUnusableMoves(BattleTarget target)
    {
        throw new System.NotImplementedException();
    }
}