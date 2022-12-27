using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsywaveDamage : EffectDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int damage = (user.pokemon.level * (Random.Range(0, 101) + 50)) / 100;
        Debug.Log(damage);
        yield return StartCoroutine(ApplyDamage(moveData, user, target, damage));
    }
}
