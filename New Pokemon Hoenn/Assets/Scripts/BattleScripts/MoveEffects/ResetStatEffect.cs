using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatEffect : MoveEffect
{
    public bool[] statsReset = new bool[8];
    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
