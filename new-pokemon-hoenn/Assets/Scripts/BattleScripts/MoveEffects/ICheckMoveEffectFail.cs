using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckMoveEffectFail
{
    public abstract bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData);
}
