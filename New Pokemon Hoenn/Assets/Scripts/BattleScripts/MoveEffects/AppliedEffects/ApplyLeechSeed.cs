using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyLeechSeed : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.inflictor.pokemon.primaryStatus != PrimaryStatus.Fainted){
            int leechSeedHealth = (int)(target.pokemon.stats[0] * 0.1f);
            yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, -leechSeedHealth));
            target.pokemon.CurrentHealth -= leechSeedHealth;

            //check for liquid ooze
            yield return StartCoroutine(effectInfo.inflictor.battleHUD.healthBar.SetHealthBar(effectInfo.inflictor.pokemon, leechSeedHealth));
            effectInfo.inflictor.pokemon.CurrentHealth += leechSeedHealth;

            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s health is sapped by leech seed!"));
        }
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return target.pokemon.IsThisType(StatLib.Type.Grass);
    }
}