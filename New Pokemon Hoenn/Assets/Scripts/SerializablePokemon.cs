using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePokemon
{
    public PokemonDefault pokemonDefault;
    public StatLib.Ability ability;
    public bool isShiny;
    public ItemDataSO ballUsed;
    [Range(0, 252)] public int[] effortValues = new int[6];
    [Range(0, 31)] public int[] individualValues = new int[6];
    public int level;
    public int friendship;
    public Gender gender;
    public float height;
    public float weight;
    public ItemDataSO heldItem;
    [Tooltip("Only the moves you want to specify are necessary")]
    public List<GameObject> moves;
    [Tooltip("Leaving 0 will make the PP whatever the max is for the move at the corresponding index")]
    public int[] moveMaxPP = new int[4];
    public Nature nature;
}
