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

    public static GameObject GetItemPrefab(ItemData itemData){
        return Instance.itemPrefabManifest.Find(g => g.GetComponent<ItemLogic>().itemData == itemData);
    }

    public static IEnumerable<KeyValuePair<ItemData, int>> GetEnumerableKeyValuePairs(){
        return Instance.inventory as IEnumerable<KeyValuePair<ItemData, int>>;
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

    void Awake(){
        if(Instance != null){
            Debug.Log("PlayerInventory exists");
            return;
        }
        Instance = this;
    }

    void Start(){
        inventory = new Dictionary<ItemData, int>();
        inventory.Add(giveOnStart, 1);
        //load inventory from save file here?
    }
}
