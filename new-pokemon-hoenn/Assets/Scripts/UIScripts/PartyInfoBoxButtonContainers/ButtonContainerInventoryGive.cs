using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerInventoryGive : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject giveButton;
    public override void LoadActionButtons(Pokemon p){
        giveButton.SetActive(p.heldItem == null);
    }

    public void GiveButtonFunction(){
        DisableAlternateInputs();
        StartCoroutine(GiveItem());
    }

    private IEnumerator GiveItem(){
        PartyInfoBox boxComponent = GetComponent<PartyInfoBox>();
        Pokemon monToGiveItem = PlayerParty.Instance.playerParty.party[boxComponent.whichPartyMember];
        monToGiveItem.heldItem = InventoryMenu.LoadedItemInstance.itemData;
        PlayerInventory.SubtractItem(monToGiveItem.heldItem);
        boxComponent.LoadPokemonDetails(false);
        string message = monToGiveItem.nickName + " was given the " + monToGiveItem.heldItem.itemName + " to hold";
        yield return StartCoroutine(modal.ShowModalMessage(message));
        GameObject.FindWithTag("InventoryMenu").GetComponentInChildren<InventoryMenu>().UpdateBadge(monToGiveItem.heldItem);
        giveButton.GetComponent<OverlayTransitionCaller>().CallTransition();
    }
}
