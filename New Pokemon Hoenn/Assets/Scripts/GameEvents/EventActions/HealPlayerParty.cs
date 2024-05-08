using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerParty : EventAction
{
    protected override IEnumerator EventActionLogic(){
        foreach (Pokemon p in PlayerParty.Instance.playerParty.party){
            p.CurrentHealth = p.stats[0];
            p.primaryStatus = PrimaryStatus.None;
            for(int i = 0; i < p.moves.Count; i++){
                p.movePP = p.moveMaxPP;
            }
        }
        yield break;
    }
}
