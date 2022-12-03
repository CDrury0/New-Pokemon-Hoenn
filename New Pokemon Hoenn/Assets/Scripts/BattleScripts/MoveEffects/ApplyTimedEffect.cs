using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTimedEffect : MoveEffect, IApplyEffect
{
    public int timer;
    public GameObject moveUsed;

    public IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
