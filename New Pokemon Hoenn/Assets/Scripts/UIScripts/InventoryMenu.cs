using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private Transform scrollContent;
    [SerializeField] private GameObject itemBadgePrefab;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject giveButton;
    [SerializeField] private GameObject sellButton;
    public static ItemLogic LoadedItemInstance { get; private set; }

    void OnEnable(){
        foreach(Tuple<ItemData, int> invEntry in PlayerInventory.Instance.inventory){
            GameObject itemBadge = Instantiate(itemBadgePrefab, Vector3.zero, Quaternion.identity, scrollContent);
            itemBadge.GetComponent<ItemBadgeButton>().Load(invEntry.Item1, invEntry.Item2, () => { LoadItem(invEntry.Item1); });
        }
    }

    void OnDisable(){
        LoadedItemInstance = null;
        Destroy(transform.parent.gameObject);
    }

    public void LoadItem(ItemData itemData){
        LoadedItemInstance = Instantiate(PlayerInventory.GetItemPrefab(itemData)).GetComponent<ItemLogic>();
        itemDescription.text = itemData.itemDescription;
        bool allowUse = (CombatSystem.BattleActive && LoadedItemInstance.onUseDuringBattle.Count > 0)
        || (!CombatSystem.BattleActive && LoadedItemInstance.onUseOutsideBattle.Count > 0);
        useButton.SetActive(allowUse);
        giveButton.SetActive(LoadedItemInstance.heldItem != null);

        //also allow filtering based on pockets, alphabetical, order obtained, etc.
        //use static variables for persistence between inventory menu instances
    }

    public static bool CanLoadedItemBeUsedOn(Pokemon p){
        foreach(ItemEffect i in LoadedItemInstance.onUseOutsideBattle){
            //if ANY effect cannot be used, the item cannot be used
            if(!i.CanEffectBeUsed(p)){
                return false;
            }
        }
        return true;
    }
}
