using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public DynamicDictionary<string, Vector3>.Entry currentPosition;
    public DynamicDictionary<string, Vector3>.Entry lastHealedPosition;
    public DynamicDictionary<string, Vector3>.Entry escapePosition;
    public int movementMode;
    public List<SaveablePokemon> currentParty;
    public List<SaveablePokemon> boxMons;
    public DexStatus[] dexStatus;
    public List<DynamicDictionary<int, int>.Entry> inventory;
    public List<string> playerBadges;
    //step event modifiers
    public List<DynamicDictionary<string, List<int>>.Entry> gameAreaEventManifests;
    
    // event status for each tracked "quest" sequence   
}
