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
    public GameObject sendOutButton;
    [SerializeField] private Image battleHUDImage;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color faintedColor;
    
    public void LoadPokemonDetails(){
        Pokemon pokemonToDisplay = PlayerParty.Instance.playerParty.party[whichPartyMember];
        bool enableInfoBox = pokemonToDisplay != null;
        boxSprite.gameObject.SetActive(enableInfoBox);
        pokemonInfo.gameObject.SetActive(enableInfoBox);
        actionButtonPanel.SetActive(enableInfoBox);
        if(enableInfoBox){
            boxSprite.sprite = pokemonToDisplay.boxSprite;
            pokemonInfo.SetBattleHUD(pokemonToDisplay);
            battleHUDImage.color = pokemonToDisplay.primaryStatus == PrimaryStatus.Fainted ? faintedColor : normalColor;
        }

        ActivateSendOutButton();
    }

    private void ActivateSendOutButton(){
        sendOutButton.SetActive(false);
        if(CombatSystem.BattleActive){
            sendOutButton.SetActive(MoveFunctions.CanBeSwitchedIn(PlayerParty.Instance.playerParty.party[whichPartyMember]));
        }
    }

    public void SendOutButtonFunction(){
        GetComponentInParent<PartyMenu>().gameObject.SetActive(false);
        CombatSystem.ActiveTarget.individualBattleModifier.switchingIn = PlayerParty.Instance.playerParty.party[whichPartyMember];
        CombatSystem.Proceed = true;
    }
}
