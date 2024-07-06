using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference Objects/SaveManager")]
public class SaveManager : ScriptableObject
{
    public static SaveManager Instance { get; private set; }

    public void LoadData(){
        string data = File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(data);
        Debug.Log(saveData.currentPosition.value);
    }

    public void SaveData(){
        SaveData saveData = new() {
            currentPosition = new(ReferenceLib.ActiveArea.name, PlayerInput.playerTransform.position),
            lastHealedPosition = new(ReferenceLib.LastHealPosition.key?.name ?? "null", ReferenceLib.LastHealPosition.value),
            escapePosition = new(ReferenceLib.EscapePosition.key?.name ?? "null", ReferenceLib.EscapePosition.value),
            currentParty = GetSaveablePokemonList(PlayerParty.Instance.playerParty.party),
            dexStatus = ReferenceLib.GlobalDexProgress,
            inventory = GetSerializableInventoryList(PlayerInventory.GetKeyValuePairList()),
            gameAreaEventManifests = GetSerializableEventManifest(),
        };
        
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", data);
        Debug.Log("Game data saved");
    }

    public static List<DynamicDictionary<string, List<int>>.Entry> GetSerializableEventManifest(){
        List<AreaData> allAreas = Resources.LoadAll<AreaData>("AreaDataObjects/").ToList();
        Debug.Log(allAreas.Count);
        List<DynamicDictionary<string, List<int>>.Entry> newList = new();
        foreach(AreaData area in allAreas){
            newList.Add(new(area.name, new List<int>(area.eventManifest)));
        }
        return newList;
    }

    public static List<SaveablePokemon> GetSaveablePokemonList(Pokemon[] party){
        List<SaveablePokemon> newList = new(party.Length);
        foreach(Pokemon p in party){
            if (p != null){
                newList.Add(SaveablePokemon.GetSaveablePokemon(p));
            }
        }
        return newList;
    }

    public static List<DynamicDictionary<int, int>.Entry> GetSerializableInventoryList(List<KeyValuePair<ItemData, int>> kvp) {
        List<DynamicDictionary<int, int>.Entry> newList = new(kvp.Count);
        for(int i = 0; i < kvp.Count; i++){
            newList.Add(new(PlayerInventory.GetItemID(kvp[i].Key), kvp[i].Value));
        }
        return newList;
    }

    public void Awake(){
        if(Instance != null){
            Debug.LogWarning("Instance of SaveManager already exists");
            return;
        }
        Instance = this;
    }
}
