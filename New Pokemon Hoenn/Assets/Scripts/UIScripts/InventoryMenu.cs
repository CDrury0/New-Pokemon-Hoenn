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
    private ItemLogic loadedItemInstance;

    void OnEnable(){
        foreach(Tuple<ItemData, int> invEntry in PlayerInventory.Instance.inventory){
            GameObject itemBadge = Instantiate(itemBadgePrefab, Vector3.zero, Quaternion.identity, scrollContent);
            itemBadge.GetComponent<ItemBadgeButton>().Load(invEntry.Item1, invEntry.Item2, () => { LoadItem(invEntry.Item1); });
        }
    }

    void OnDisable(){
        Destroy(transform.parent.gameObject);
    }

    public void LoadItem(ItemData itemData){
        loadedItemInstance = Instantiate(PlayerInventory.GetItemPrefab(itemData)).GetComponent<ItemLogic>();
        itemDescription.text = itemData.itemDescription;
        //load buttons that allow use based on loadedItemInstance
        //also allow filtering based on pockets, alphabetical, order obtained, etc.
        //use static variables for persistence between inventory menu instances
    }
}
