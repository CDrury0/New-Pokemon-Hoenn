using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void SwitchPlayerPartyMembers(int shift) {
        var party = PlayerParty.Instance.playerParty.party;
        int offset = whichPartyMember + shift;
        if(offset >= 0 && offset <= party.Length - 1 && party[offset] != null){
            (party[whichPartyMember], party[offset]) = (party[offset], party[whichPartyMember]);
            List<PartyInfoBox> siblings = transform.parent.gameObject.GetComponentsInChildren<PartyInfoBox>().ToList();
            siblings.Find((sibling) => sibling.whichPartyMember == offset)?.LoadPokemonDetails();
        }
        LoadPokemonDetails();
    }
}
