using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public DynamicDictionary<string, Vector3>.Entry currentPosition;
    //public static GameObject CurrentAreaEventObjectPrefab { get; private set; }
    public DynamicDictionary<string, Vector3>.Entry lastHealedPosition;
    public DynamicDictionary<string, Vector3>.Entry escapePosition;
    public List<SaveablePokemon> currentParty;
    //box mons
    public DexStatus[] dexStatus;
    public List<DynamicDictionary<int, int>.Entry> inventory;
    //trainer card data
    //step event modifiers
    public List<DynamicDictionary<string, List<int>>.Entry> gameAreaEventManifests;
    // event status for each tracked "quest" sequence
    // -- get all assets from folder, store key of name with val of val    
}
