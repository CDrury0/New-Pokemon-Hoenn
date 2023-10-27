using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerInventoryUse : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject useButton;
    
    public override void LoadActionButtons(Pokemon p){
        useButton.SetActive(InventoryMenu.LoadedItemInstance.CanItemBeUsedOn(p));
    }

    public void UseButtonFunction(){
        DisableAlternateInputs();
        StartCoroutine(UseItem());
    }

    private IEnumerator UseItem(){
        PartyInfoBox infoBox = GetComponent<PartyInfoBox>();
        infoBox.LoadPokemonDetails(false);
        Pokemon p = PlayerParty.Instance.playerParty.party[infoBox.whichPartyMember];
        yield return StartCoroutine(InventoryMenu.LoadedItemInstance.DoItemEffects(infoBox.pokemonInfo, p, (string message) => ShowModalMessage(message)));
        InventoryMenu invMenu = GameObject.FindWithTag("InventoryMenu").GetComponentInChildren<InventoryMenu>();
        invMenu.UpdateBadge(InventoryMenu.LoadedItemInstance.itemData);
        yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => { 
            invMenu.gameObject.SetActive(!CombatSystem.BattleActive);
            partyMenu.gameObject.SetActive(false);
            if(CombatSystem.BattleActive){
                CombatSystem.ActiveTarget.turnAction = CombatLib.Instance.combatSystem.playerUsedItemPlaceholder;
                CombatSystem.Proceed = true;
            }
        }));
    }
}
