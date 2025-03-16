using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerParty : EventAction
{
    [SerializeField] private bool setLastHealPosition;

    protected override IEnumerator EventActionLogic(EventState state){
        foreach (Pokemon p in PlayerParty.Instance.playerParty.party){
            if(p == null){
                continue;
            }
            p.CurrentHealth = p.stats[0];
            p.primaryStatus = PrimaryStatus.None;
            for(int i = 0; i < p.moves.Count; i++){
                p.movePP[i] = p.moveMaxPP[i];
            }
        }

        if(setLastHealPosition){
            ReferenceLib.SetLastHealPosition();
        }

        yield break;
    }
}
