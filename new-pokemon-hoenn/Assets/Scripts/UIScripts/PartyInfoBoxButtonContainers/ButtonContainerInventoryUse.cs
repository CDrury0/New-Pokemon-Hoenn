using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerInventoryUse : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject useButton;
    
    public override void LoadActionButtons(Pokemon p){
        useButton.SetActive(InventoryMenu.LoadedItemInstance.CanItemBeUsedOn(p));
    }

    public void UseButtonFunction(int whichMove = -1){
        DisableAlternateInputs();
        StartCoroutine(UseItem(whichMove));
    }

    private IEnumerator UseItem(int move){
        PartyInfoBox infoBox = GetComponent<PartyInfoBox>();
        infoBox.LoadPokemonDetails(false);
        Pokemon p = PlayerParty.Party.members[infoBox.whichPartyMember];
        PlayerInventory.SubtractItem(InventoryMenu.LoadedItemInstance.itemData);
        InventoryMenu invMenu = GameObject.FindWithTag("InventoryMenu").GetComponentInChildren<InventoryMenu>();
        invMenu.UpdateBadge(InventoryMenu.LoadedItemInstance.itemData);
        yield return StartCoroutine(InventoryMenu.LoadedItemInstance.DoItemEffects(
            infoBox.pokemonInfo,
            p,
            (string message) => modal.ShowModalMessage(message),
            move
        ));

        //if HandleEvolution.EvolveMon finds a party menu to destroy, this function will exit here, effectively skipping the transition
        OverlayTransitionManager.Instance.DoTransitionWithAction(
            () => {
                invMenu.gameObject.SetActive(!CombatSystem.BattleActive);
                partyMenu.gameObject.SetActive(false);
                if(CombatSystem.BattleActive){
                    CombatSystem.ActiveTarget.turnAction = CombatLib.CombatSystem.playerUsedItemPlaceholder;
                    CombatLib.CombatScreen.battleOptionsLayoutObject.SetActive(false);
                }
            },
            () => CombatSystem.Proceed = CombatSystem.BattleActive
        );
    }
}
