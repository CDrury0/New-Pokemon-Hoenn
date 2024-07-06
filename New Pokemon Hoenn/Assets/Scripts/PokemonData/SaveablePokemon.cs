using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveablePokemon
{
    public int pokemonDefaultID;
    public int abilityIndex;
    public bool isShiny;
    public int ballUsedID;
    public List<int> effortValues;
    public List<int> individualValues;
    public int level;
    public int friendship;
    public Gender gender;
    public float height;
    public float weight;
    public int heldItemID;
    public List<int> moveIDs;
    public List<int> movePP;
    public List<int> moveMaxPP;
    public string natureName;

    public static SaveablePokemon GetSaveablePokemon(Pokemon p){
        return new() {
            pokemonDefaultID = p.pokemonDefault.IDNumber,
            abilityIndex = p.abilitySlot,
            isShiny = p.isShiny,
            ballUsedID = PlayerInventory.GetItemID(p.ballUsed),
            effortValues = new List<int>(p.effortValues),
            individualValues = new List<int>(p.individualValues),
            level = p.level,
            friendship = p.Friendship,
            gender = p.gender,
            height = p.height,
            weight = p.weight,
            heldItemID = PlayerInventory.GetItemID(p.heldItem),
            moveIDs = new List<GameObject>(p.moves).Select(m => ReferenceLib.Instance.moveManifest.IndexOf(m)).ToList(),
            movePP = new List<int>(p.movePP),
            moveMaxPP = new List<int>(p.moveMaxPP),
            natureName = p.nature.name,
        };
    }
}
