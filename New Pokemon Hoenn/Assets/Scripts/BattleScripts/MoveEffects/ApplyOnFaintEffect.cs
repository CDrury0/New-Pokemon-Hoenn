using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnFaintEffect {DestinyBond, Grudge}
public class ApplyOnFaintEffect : MoveEffect, IApplyEffect
{
    public OnFaintEffect onFaintEffect;
    public IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
