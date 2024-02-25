using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyLeechSeed : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.inflictor.pokemon.primaryStatus != PrimaryStatus.Fainted){
            int leechSeedHealth = (int)(target.pokemon.stats[0] * 0.1f);
            yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -leechSeedHealth));

            //check for liquid ooze
            yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(effectInfo.inflictor, leechSeedHealth));

            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + "'s health is sapped by leech seed!"));
        }
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return target.pokemon.IsThisType(ReferenceLib.GetPokemonType("Grass"));
    }

    void Awake(){
        message = "&targetName was seeded!";
    }
}