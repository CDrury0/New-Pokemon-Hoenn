using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePokemon
{
    public PokemonDefault pokemonDefault;
    public StatLib.Ability ability;
    public bool isShiny;
    public Item ballUsed;
    public int[] effortValues;
    public int[] individualValues;
    public int level;
    public int friendship;
    public Gender gender;
    public float height;
    public float weight;
    public Item heldItem;
    public List<GameObject> moves;
    public int[] moveMaxPP = new int[4];
    public Nature nature;
}
