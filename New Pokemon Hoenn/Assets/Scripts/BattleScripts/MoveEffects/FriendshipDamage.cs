using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipDamage : EffectDamage
{
    public PiecewiseDamagePair[] piecewiseDamage;
    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
