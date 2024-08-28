using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    public GameObject promptTextBox;
    public PartyInfoBox[] infoBoxes;
    public GameObject closeButton;

    void OnDisable(){
        Destroy(transform.parent.gameObject);
    }

    void OnEnable(){
        foreach(PartyInfoBox infoBox in infoBoxes){
            infoBox.LoadPokemonDetails();
        }
    }

    /// <summary>
    /// Closing the menu is already allowed by default. Only use this if you intend to disallow it, or allow it again after disabling previously
    /// </summary>
    public void SetAllowClose(bool value){
        closeButton.SetActive(value);
    }

    public void WriteTextPrompt(string message){
        promptTextBox.GetComponentInChildren<TextMeshProUGUI>().text = message;
        promptTextBox.SetActive(true);
    }
}
