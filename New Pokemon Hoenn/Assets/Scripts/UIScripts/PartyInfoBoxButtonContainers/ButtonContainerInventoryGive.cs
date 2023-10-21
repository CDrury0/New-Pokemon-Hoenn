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
        foreach(PartyInfoBox box in GetComponentInParent<PartyMenu>().infoBoxes){
            box.actionButtonPanel.SetActive(false);
        }
        StartCoroutine(GiveItem());
    }

    private IEnumerator GiveItem(){
        PartyInfoBox boxComponent = GetComponent<PartyInfoBox>();
        Pokemon monToGiveItem = PlayerParty.Instance.playerParty.party[boxComponent.whichPartyMember];
        monToGiveItem.heldItem = InventoryMenu.LoadedItemInstance.itemData;
        PlayerInventory.SubtractItem(monToGiveItem.heldItem);
        boxComponent.LoadPokemonDetails();
        string message = monToGiveItem.nickName + " was given the " + monToGiveItem.heldItem.itemName + " to hold";
        GameObject outputInstance = Instantiate(messageModalPrefab);
        IEnumerator writeMessage = outputInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message);
        yield return StartCoroutine(writeMessage);
        Destroy(outputInstance);
        GameObject.FindWithTag("InventoryMenu").GetComponentInChildren<InventoryMenu>().UpdateBadge(monToGiveItem.heldItem);
        giveButton.GetComponent<OverlayTransitionCaller>().CallTransition();
    }
}
