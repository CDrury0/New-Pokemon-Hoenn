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
    
    void OnEnable(){
        gameObject.GetComponentInParent<Canvas>().sortingOrder = 2;
        Pokemon pokemonToDisplay = PlayerParty.Instance.playerParty.party[whichPartyMember];
        if(pokemonToDisplay == null){
            boxSprite.gameObject.SetActive(false);
            pokemonInfo.gameObject.SetActive(false);
            actionButtonPanel.SetActive(false);
            return;
        }
        boxSprite.sprite = pokemonToDisplay.boxSprite;
        pokemonInfo.SetBattleHUD(pokemonToDisplay);
        boxSprite.gameObject.SetActive(true);
        pokemonInfo.gameObject.SetActive(true);
        actionButtonPanel.SetActive(true);
        if(CombatSystem.BattleActive){
            sendOutButton.SetActive(CanBeSwitchedIn(pokemonToDisplay));
        }
        else{
            sendOutButton.SetActive(false);
        }
    }

    void OnDisable(){
        gameObject.GetComponentInParent<Canvas>().sortingOrder = 0;
    }

    public void SendOutButtonFunction(){
        //call CombatSystem method to select switch action, etc.
    }

    private bool CanBeSwitchedIn(Pokemon pokemonInThisSlot){
        return CombatSystem.ActiveTargetCanSwitch() && pokemonInThisSlot.primaryStatus != PrimaryStatus.Fainted && !pokemonInThisSlot.inBattle;
    }
}
