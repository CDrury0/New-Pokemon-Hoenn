using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [Tooltip("For use in Unity inspector; see SaveablePokemon for fully System serializable implementation")]
public class SerializablePokemon
{
    public PokemonDefault pokemonDefault;
    public StatLib.Ability ability;
    public bool isShiny;
    public ItemData ballUsed;
    [Tooltip("Automatically calculated based on effortValues below")] public int evTotal;
    [Range(0, Pokemon.MAX_EV)] public int[] effortValues = new int[6];
    [Range(0, Pokemon.MAX_IV)] public int[] individualValues = new int[6];
    public int level;
    public int friendship;
    public Gender gender;
    public float height;
    public float weight;
    public ItemData heldItem;
    [Tooltip("Only the moves you want to specify are necessary")]
    public List<GameObject> moves;
    [Tooltip("Leaving 0 will make the PP whatever the max is for the move at the corresponding index")]
    public int[] moveMaxPP = new int[4];
    [Tooltip("Null reference will result in a neutral nature when Pokemon is generated")]
    public PokemonNature nature;
}
