using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveablePokemon
{
    public int numberID;
    public int pokemonDefaultID;
    public string nickName;
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
    public int currentHealth;
    public PrimaryStatus primaryStatus;
    public int hiddenPowerType;
    public int experience;
    public List<int> moveIDs;
    public List<int> movePP;
    public List<int> moveMaxPP;
    public string natureName;
    public string metArea;
    public int metLevel;

    public static SaveablePokemon GetSaveablePokemon(Pokemon p){
        return new() {
            numberID = p.numberID,
            pokemonDefaultID = p.pokemonDefault.IDNumber,
            nickName = p.nickName,
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
            currentHealth = p.CurrentHealth,
            primaryStatus = p.primaryStatus,
            hiddenPowerType = ReferenceLib.Instance.typeList.IndexOf(p.hiddenPowerType),
            experience = p.experience,
            moveIDs = new List<GameObject>(p.moves).Select(m => m == null ? -1 : ReferenceLib.Instance.moveManifest.IndexOf(m)).ToList(),
            movePP = new List<int>(p.movePP),
            moveMaxPP = new List<int>(p.moveMaxPP),
            natureName = p.nature.name,
            metArea = p.metArea,
            metLevel = p.metLevel
        };
    }
}
