using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartyInfoBoxButtonContainer : MonoBehaviour
{
    [SerializeField] protected ModalMessage modal;
    protected PartyMenu partyMenu;
    public abstract void LoadActionButtons(Pokemon p);
    
    protected void DisableAlternateInputs(){
        partyMenu.closeButton.SetActive(false);
        foreach(PartyInfoBox box in partyMenu.infoBoxes){
            box.actionButtonPanel.SetActive(false);
        }
    }

    void Awake(){
        partyMenu = GetComponentInParent<PartyMenu>();
    }
}
