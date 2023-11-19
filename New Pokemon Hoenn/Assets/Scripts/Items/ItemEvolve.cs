using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvolve : ItemEffect
{
    public override bool CanEffectBeUsed(Pokemon p){
        return p.pokemonDefault.evoDetails.HasStoneEntry(itemLogic.itemData);
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput){
        HandleEvolution handleEvolution = Instantiate(CombatLib.Instance.combatSystem.handleEvolutionObj).GetComponent<HandleEvolution>();
        yield return StartCoroutine(handleEvolution.EvolveMon(p, itemLogic.itemData));
        Destroy(handleEvolution.gameObject);
    }
}
