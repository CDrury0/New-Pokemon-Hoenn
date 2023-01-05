using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyNightmare : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(target.pokemon.primaryStatus == PrimaryStatus.Asleep){
            int nightmareDamage = (int)(0.25f * target.pokemon.stats[0]);
            yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, -nightmareDamage));
            target.pokemon.CurrentHealth -= nightmareDamage;
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is tormented by its nightmare!"));
        }
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return target.pokemon.primaryStatus != PrimaryStatus.Asleep;
    }
}