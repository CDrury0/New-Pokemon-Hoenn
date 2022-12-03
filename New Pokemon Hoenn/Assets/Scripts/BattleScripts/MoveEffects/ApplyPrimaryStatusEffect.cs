using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPrimaryStatusEffect : MoveEffect
{
    public PrimaryStatus statusInflicted;
    public bool toxic;
    public float chance;
    public bool powder;
    public bool ignoresExistingCondition;
    public bool triAttack;

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
