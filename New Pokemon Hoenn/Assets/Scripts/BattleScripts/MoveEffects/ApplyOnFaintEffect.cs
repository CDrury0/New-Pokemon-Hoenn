using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnFaintEffect {DestinyBond, Grudge}
public class ApplyOnFaintEffect : MoveEffect
{
    public OnFaintEffect onFaintEffect;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
