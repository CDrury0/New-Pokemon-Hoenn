using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckMoveFail
{
    public abstract string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData);
}
