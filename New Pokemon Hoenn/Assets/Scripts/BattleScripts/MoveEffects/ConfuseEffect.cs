using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseEffect : MoveEffect
{
    public float chance;
    public bool confuseSelf;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
