using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyIndividualEffect : MoveEffect, IApplyEffect
{
    public int timer;

    public abstract IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData);
}
