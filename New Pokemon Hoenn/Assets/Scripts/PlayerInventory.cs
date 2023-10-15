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
    public List<Tuple<ItemData, int>> inventory;

    public static GameObject GetItemPrefab(int itemIndex){
        return Instance.itemPrefabManifest.Find(g => g.GetComponent<ItemLogic>().itemData == Instance.dataManifest[itemIndex]);
    }

    public static GameObject GetItemPrefab(ItemData itemData){
        return Instance.itemPrefabManifest.Find(g => g.GetComponent<ItemLogic>().itemData == itemData);
    }

    void Awake(){
        if(Instance != null){
            Debug.Log("PlayerInventory exists");
            return;
        }
        Instance = this;
    }

    void Start(){
        inventory = new List<Tuple<ItemData, int>>();
        inventory.Add(new Tuple<ItemData, int>(giveOnStart, 1));
        foreach(ItemData i in dataManifest){
            inventory.Add(new Tuple<ItemData, int>(i, 1));
        }
        //load inventory from save file here?
    }
}
