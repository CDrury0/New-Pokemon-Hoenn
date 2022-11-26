using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class PiecewiseDamagePair
{
    public float key; //hp
    public int powerValue;
}
public class HealthBasedDamage : EffectDamage
{
    public PiecewiseDamagePair[] piecewiseDamage;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
