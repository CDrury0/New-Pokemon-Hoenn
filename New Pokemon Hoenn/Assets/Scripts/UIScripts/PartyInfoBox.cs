using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyInfoBox : MonoBehaviour
{
    public int whichPartyMember;
    public BattleHUD pokemonInfo;
    public Image boxSprite;
    public GameObject actionButtonPanel;
    [SerializeField] private PartyInfoBoxButtonContainer actionButtonContainer;
    [SerializeField] private Image battleHUDImage;
    [SerializeField] private GameObject heldItemSprite;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color faintedColor;
    
    public void LoadPokemonDetails(bool showActionButtons = true){
        Pokemon pokemonToDisplay = PlayerParty.Instance.playerParty.party[whichPartyMember];
        bool enableInfoBox = pokemonToDisplay != null;
        boxSprite.gameObject.SetActive(enableInfoBox);
        pokemonInfo.gameObject.SetActive(enableInfoBox);
        actionButtonPanel.SetActive(enableInfoBox && showActionButtons);
        if(enableInfoBox){
            heldItemSprite.SetActive(pokemonToDisplay.heldItem != null);
            boxSprite.sprite = pokemonToDisplay.boxSprite;
            pokemonInfo.SetBattleHUD(pokemonToDisplay);
            battleHUDImage.color = pokemonToDisplay.primaryStatus == PrimaryStatus.Fainted ? faintedColor : normalColor;
            actionButtonContainer.LoadActionButtons(pokemonToDisplay);
        }
    }
}
