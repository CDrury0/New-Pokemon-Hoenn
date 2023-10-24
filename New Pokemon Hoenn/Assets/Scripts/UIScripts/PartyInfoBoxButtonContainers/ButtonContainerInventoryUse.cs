using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryMenu;

public class ButtonContainerInventoryUse : PartyInfoBoxButtonContainer
{
    [SerializeField] private GameObject useButton;
    public override void LoadActionButtons(Pokemon p){
        useButton.SetActive(CanLoadedItemBeUsedOn(p));
    }

    private bool CanLoadedItemBeUsedOn(Pokemon p){
        IEnumerable<ItemEffect> itemEffects = CombatSystem.BattleActive ? LoadedItemInstance.onUseDuringBattle : LoadedItemInstance.onUseOutsideBattle;
        foreach(ItemEffect i in itemEffects){
            //if ANY effect cannot be used, the item cannot be used
            if(!i.CanEffectBeUsed(p)){
                return false;
            }
        }
        return true;
    }

    public void UseButtonFunction(){
        StartCoroutine(UseItem());
    }

    private IEnumerator UseItem(){
        PartyInfoBox infoBox = GetComponent<PartyInfoBox>();
        infoBox.LoadPokemonDetails(false);
        Pokemon p = PlayerParty.Instance.playerParty.party[infoBox.whichPartyMember];
        yield return StartCoroutine(InventoryMenu.LoadedItemInstance.DoItemEffects(infoBox.pokemonInfo, p, (string message) => MessageCallback(message)));
        InventoryMenu invMenu = GameObject.FindWithTag("InventoryMenu").GetComponentInChildren<InventoryMenu>();
        invMenu.UpdateBadge(LoadedItemInstance.itemData);
        yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => { 
            invMenu.gameObject.SetActive(!CombatSystem.BattleActive);
            GetComponentInParent<PartyMenu>().gameObject.SetActive(false);
            CombatSystem.Proceed = CombatSystem.BattleActive;
        }));
    }

    private IEnumerator MessageCallback(string message){
        GameObject outputInstance = Instantiate(messageModalPrefab);
        WriteText writeText = outputInstance.GetComponentInChildren<WriteText>();
        yield return StartCoroutine(writeText.WriteMessageConfirm(message));
        Destroy(outputInstance.gameObject);
    }
}
