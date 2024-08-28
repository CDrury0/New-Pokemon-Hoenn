using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDamage : EffectDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(ApplyDamage(moveData, user, target, user.pokemon.level));
    }
}
