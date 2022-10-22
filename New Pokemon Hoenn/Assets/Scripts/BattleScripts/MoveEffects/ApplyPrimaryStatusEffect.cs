using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPrimaryStatusEffect : ApplyEffect
{
    public PrimaryStatus statusInflicted;
    public bool toxic;
    public float chance;
    public bool powder;
    public bool ignoresExistingCondition;
    public bool triAttack;
    public int duration; //if duration is 0, apply standard duration

    public override IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
