using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndeavorDamage : EffectDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int damage = target.pokemon.CurrentHealth - user.pokemon.CurrentHealth;
        yield return StartCoroutine(ApplyDamage(moveData, user, target, damage));
    }

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData){
        if(user.pokemon.CurrentHealth >= target.pokemon.CurrentHealth){
            return "But it failed!";
        }
        return base.CheckMoveFail(user, target, moveData);
    }
}
