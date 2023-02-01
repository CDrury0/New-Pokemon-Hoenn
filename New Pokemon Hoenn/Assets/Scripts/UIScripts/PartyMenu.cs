using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    public TextMeshProUGUI partyPromptText;
    public PartyInfoBox[] infoBoxes;
    public GameObject closeButton;

    void OnEnable(){
        GetComponentInParent<Canvas>().sortingOrder = 1;
    }

    void OnDisable(){
        GetComponentInParent<Canvas>().sortingOrder = 0;
    }

    public void OpenParty(bool allowClose){
        foreach(PartyInfoBox infoBox in infoBoxes){
            infoBox.LoadPokemonDetails();
        }
        closeButton.SetActive(allowClose);
        gameObject.SetActive(true);
    }
}
