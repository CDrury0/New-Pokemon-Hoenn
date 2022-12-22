using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEffect : MoveEffect
{
    public float chance;
    public int[] statChanges = new int[8];
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
