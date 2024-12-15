using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerParty : EventAction
{
    [SerializeField] private bool setLastHealPosition;

    public override void DoEventAction(EventState chainState) {
        foreach (Pokemon p in PlayerParty.Instance.playerParty.party)
            p?.HealComplete();            

        if(setLastHealPosition)
            ReferenceLib.SetLastHealPosition();

        NextAction(chainState);
    }
}
