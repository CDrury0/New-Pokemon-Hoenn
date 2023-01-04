using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCurse : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget user, AppliedEffectInfo effectInfo)
    {
        int curseDamage = (int)(0.25f * user.pokemon.stats[0]);
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon, -curseDamage));
        user.pokemon.CurrentHealth -= curseDamage;
        yield return StartCoroutine(user.GetName() + "is afflicted by its curse!");
    }
}
