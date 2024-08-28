using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage("But nothing happened!"));
    }
}
