using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    [SerializeField] private ItemData giveOnStart;
    [SerializeField] private List<ItemData> dataManifest;
    [SerializeField] private List<GameObject> itemPrefabManifest;
    private Dictionary<ItemData, int> inventory;

    public static GameObject GetItemPrefab(int itemIndex){
        return Instance.itemPrefabManifest.Find(g => g.GetComponent<ItemLogic>().itemData == Instance.dataManifest[itemIndex]);
    }

    /// <returns>A copy of the player inventory as a list of key/value pairs</returns>
    public static List<KeyValuePair<ItemData, int>> GetKeyValuePairList(){
        return new List<KeyValuePair<ItemData,int>>(Instance.inventory);
    }

    public static ItemData GetItemData(int index){
        return Instance.dataManifest[index];
    }

    public static int GetInventoryQuantity(ItemData itemData){
        return Instance.inventory.ContainsKey(itemData) ? Instance.inventory[itemData] : 0;
    }

    public static void SubtractItem(ItemData itemData){
        if(Instance.inventory.ContainsKey(itemData)){
            Instance.inventory[itemData]--;
            if(Instance.inventory[itemData] == 0){
                Instance.inventory.Remove(itemData);
            }
        }
    }

    public static void AddItem(ItemData itemData, int qty = 1){
        if(!Instance.inventory.TryAdd(itemData, qty)){
            Instance.inventory[itemData] += qty;
        }
    }

    void Awake(){
        if(Instance != null){
            Debug.Log("PlayerInventory exists");
            return;
        }
        Instance = this;
    }

    void Start(){
        inventory = new Dictionary<ItemData, int>();
        foreach(ItemData i in dataManifest){
            AddItem(i);
        }
        AddItem(giveOnStart, 3);
        //load inventory from save file here?
    }
}
