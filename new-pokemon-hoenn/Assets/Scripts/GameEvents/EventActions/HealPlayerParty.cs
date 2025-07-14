using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerParty : EventAction
{
    [SerializeField] private bool setLastHealPosition;

    protected override IEnumerator EventActionLogic(){
        foreach (Pokemon p in PlayerParty.Party.members){
            p.CurrentHealth = p.stats[0];
            p.primaryStatus = PrimaryStatus.None;
            for(int i = 0; i < p.moves.Count; i++){
                p.movePP[i] = p.moveMaxPP[i];
            }
        }
        if(setLastHealPosition)
            ReferenceLib.SetLastHealPosition();

        yield break;
    }
}
