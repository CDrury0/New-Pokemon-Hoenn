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
        Debug.Log("gave item");
    }
}
