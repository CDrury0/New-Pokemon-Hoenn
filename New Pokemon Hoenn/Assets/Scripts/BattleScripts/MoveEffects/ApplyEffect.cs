using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyEffect : MoveEffect
{
    public MoveEffect endTurnEffect;
    public int timer = 10000;
    public abstract IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData);
}
