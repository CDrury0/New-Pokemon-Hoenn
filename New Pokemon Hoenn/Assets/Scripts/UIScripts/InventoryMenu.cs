using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private Transform scrollContent;
    [SerializeField] private GameObject itemBadgePrefab;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject giveButton;
    [SerializeField] private GameObject sellButton;
    private const string DEFAULT_DESCRIPTION = "Select an item to learn more about it.";
    private List<ItemBadgeButton> itemBadgeButtons;
    private ItemData selectedItemData;
    public static ItemLogic LoadedItemInstance { get; private set; }

    void OnEnable(){
        itemBadgeButtons = new List<ItemBadgeButton>();
        foreach (KeyValuePair<ItemData, int> kvp in PlayerInventory.GetEnumerableKeyValuePairs()){
            ItemBadgeButton itemBadge = Instantiate(itemBadgePrefab, Vector3.zero, Quaternion.identity, scrollContent).GetComponent<ItemBadgeButton>();
            itemBadge.Load(kvp.Key, kvp.Value, () => { LoadItem(kvp.Key); });
            itemBadgeButtons.Add(itemBadge);
        }
    }

    void OnDisable(){
        if(LoadedItemInstance != null){
            Destroy(LoadedItemInstance.gameObject);
        }
        LoadedItemInstance = null;
        Destroy(transform.parent.gameObject);
    }

    private void SetDescriptionPanel(string description = DEFAULT_DESCRIPTION, bool allowUse = false, bool allowGive = false){
        itemDescription.text = description;
        useButton.SetActive(allowUse);
        giveButton.SetActive(allowGive);
    }

    public void UpdateBadge(ItemData itemData){
        ItemBadgeButton b = itemBadgeButtons.Find(i => i.itemData == itemData);
        int numAvailable = PlayerInventory.GetInventoryQuantity(itemData);
        SetDescriptionPanel();
        if(numAvailable == 0){
            itemBadgeButtons.Remove(b);
            Destroy(b.gameObject);
        }
        else{
            b.Load(itemData, numAvailable);
        }
    }

    public void LoadItem(ItemData itemData){
        if(LoadedItemInstance != null){
            Destroy(LoadedItemInstance.gameObject);
        }
        LoadedItemInstance = itemData.itemLogicGO != null ? Instantiate(itemData.itemLogicGO).GetComponent<ItemLogic>() : null;
        bool allowUse = LoadedItemInstance != null && LoadedItemInstance.GetAllowUse();
        bool allowGive = LoadedItemInstance != null && LoadedItemInstance.heldItem != null && !CombatSystem.BattleActive;
        SetDescriptionPanel(itemData.itemDescription, allowUse, allowGive);

        //also allow filtering based on pockets, alphabetical, order obtained, etc.
        //use static variables for persistence between inventory menu instances
    }
}
