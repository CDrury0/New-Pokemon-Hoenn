using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEffect : MoveEffect
{
    public bool affectSelf;
    public float chance;
    public int[] statChanges = new int[8];
    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
