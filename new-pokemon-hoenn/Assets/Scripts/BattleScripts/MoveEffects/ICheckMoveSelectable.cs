using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckMoveSelectable
{
    public abstract List<GameObject> GetUnusableMoves(BattleTarget target);
}
