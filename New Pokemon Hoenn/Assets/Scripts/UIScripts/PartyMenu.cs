using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    public TextMeshProUGUI partyPromptText;
    public PartyInfoBox[] infoBoxes;

    void OnEnable(){
        GetComponentInParent<Canvas>().sortingOrder = 1;
    }

    void OnDisable(){
        GetComponentInParent<Canvas>().sortingOrder = 0;
    }

    public void OpenParty(){
        foreach(PartyInfoBox infoBox in infoBoxes){
            infoBox.LoadPokemonDetails();
        }
        gameObject.SetActive(true);
    }
}
