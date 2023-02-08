using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    public GameObject promptTextBox;
    public PartyInfoBox[] infoBoxes;
    public GameObject closeButton;

    void OnEnable(){
        GetComponentInParent<Canvas>().sortingOrder = 1;
    }

    void OnDisable(){
        GetComponentInParent<Canvas>().sortingOrder = 0;
    }

    public void OpenPartyNoMessage(bool allowClose){
        OpenParty(allowClose);
    }

    public void OpenParty(bool allowClose, string message = null){
        foreach(PartyInfoBox infoBox in infoBoxes){
            infoBox.LoadPokemonDetails();
        }
        closeButton.SetActive(allowClose);

        promptTextBox.SetActive(false);
        if(message != null){
            promptTextBox.GetComponentInChildren<TextMeshProUGUI>().text = message;
            promptTextBox.SetActive(true);
        }

        gameObject.SetActive(true);
    }
}
