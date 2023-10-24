using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerStandard : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject takeItemButton;

    public override void LoadActionButtons(Pokemon p){
        takeItemButton.SetActive(p.heldItem != null);
    }

    public void TakeItemButtonFunction(){
        Pokemon p = PlayerParty.Instance.playerParty.party[GetComponent<PartyInfoBox>().whichPartyMember];
        StartCoroutine(TakeItem(p));
    }

    private IEnumerator TakeItem(Pokemon p){
        PlayerInventory.AddItem(p.heldItem);
        //this message needs to use the item the pokemon was holding before being set to null
        string message = "Received the " + p.heldItem.itemName + " from " + p.nickName;
        p.heldItem = null;
        GetComponent<PartyInfoBox>().LoadPokemonDetails(true);
        yield return StartCoroutine(ShowModalMessage(message));
    }
}
