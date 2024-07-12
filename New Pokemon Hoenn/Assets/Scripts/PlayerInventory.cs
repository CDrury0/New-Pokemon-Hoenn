using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    [SerializeField] private ItemData giveOnStart;
    [SerializeField] private List<ItemData> dataManifest;
    private Dictionary<ItemData, int> inventory;

    /// <returns>A copy of the player inventory as a list of key/value pairs</returns>
    public static List<KeyValuePair<ItemData, int>> GetKeyValuePairList() => new(Instance.inventory);

    public static int GetItemID(ItemData data) => Instance.dataManifest.IndexOf(data);

    public static ItemData GetItemData(int index) => Instance.dataManifest[index];

    /// <returns>A copy of the player inventory as a dictionary</returns>
    public static Dictionary<ItemData, int> GetInventoryDictionary() => new(Instance.inventory);

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

    private static void LoadInventoryFromSave() {
        if(SaveManager.LoadedSave?.inventory == null)
            return;

        foreach(var entry in SaveManager.LoadedSave.inventory)
            AddItem(GetItemData(entry.key), entry.value);
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
        LoadInventoryFromSave();
    }
}
