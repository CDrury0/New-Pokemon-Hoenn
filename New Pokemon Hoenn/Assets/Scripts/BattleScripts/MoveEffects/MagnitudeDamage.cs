using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnitudeDamage : EffectDamage
{
    public PiecewiseDamagePair[] piecewiseDamage;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
