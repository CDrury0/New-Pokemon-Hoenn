using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerBattle : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject sendOutButton;

    public override void LoadActionButtons(Pokemon p){
        sendOutButton.SetActive(EnableSendOut(p));
    }

    private bool EnableSendOut(Pokemon p){
        return CombatLib.Instance.combatSystem.CanBeSwitchedIn(p);
    }

    public void SendOutButtonFunction(){
        int whichPartyMember = GetComponentInParent<PartyInfoBox>().whichPartyMember;
        Pokemon monSwitchingIn = PlayerParty.Instance.playerParty.party[whichPartyMember];
        CombatSystem.ActiveTarget.individualBattleModifier.switchingIn = monSwitchingIn;
        GetComponentInParent<PartyMenu>().gameObject.SetActive(false);
    }
}
