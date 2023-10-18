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
        Debug.Log("item used");
    }
}
