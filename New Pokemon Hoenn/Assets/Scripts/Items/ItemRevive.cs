using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRevive : ItemEffect
{
    public override bool CanEffectBeUsed(Pokemon p){
        return p.primaryStatus == PrimaryStatus.Fainted;
    }

    public override IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput){
        p.primaryStatus = PrimaryStatus.None;
        hudObj.GetComponentInParent<PartyInfoBox>().LoadPokemonDetails(false);
        yield break;
    }
}
