using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthRate { Slow, Medium, Fast }

[SerializeField][CreateAssetMenu]
public class PokemonDefault : ScriptableObject
{ 
    public int[] baseStats = new int[6];
    public AudioClip cry;
    public string pokemonName;
    public string species;
    public float height;
    public float weight;
    public int IDNumber;
    public Pokemon.Type type1;
    public Pokemon.Type type2;
    public int friendship;
    public int baseExperience;
    public GrowthRate growthRate;
    public int catchRate;
    public float genderRatio;
    public StatLib.EggGroup eggGroup1;
    public StatLib.EggGroup eggGroup2;
    public int eggCycles;
    public PokemonDefault childPokemon;
    public PokemonDefault evolvesInto1;
    public PokemonDefault evolvesInto2;
    public string pokedexEntry;
    public StatLib.Ability ability1;
    public StatLib.Ability ability2;
    public Sprite normalFront;
    public Sprite normalBack;
    public Sprite shinyFront;
    public Sprite shinyBack;
    public int evolutionLevel;
    public EvoDetails evoDetails;
    public Sprite boxSprite;
    public Sprite shinyBoxSprite;
    public Item naturallyHeldItem;
    public AreaData lastSeen;
    public int[] evYield = new int[6];
    public GameObject[] learnedMoves;
    public GameObject[] eggMoves;
    public GameObject[] tutorMoves;

    public int CalculateExperienceAtLevel(int level)
    {
        switch (growthRate)
        {
            case GrowthRate.Fast:
                return (int)(Mathf.Pow(level, 3) * 4 / 5);
            case GrowthRate.Medium:
                return (int)Mathf.Pow(level, 3);
            case GrowthRate.Slow:
                return (int)(Mathf.Pow(level, 3) * 5 / 4);
            default:
                return 0;
        }
    }
}

[System.Serializable]
public class EvoDetails
{
    public Item evolvesWithItem;
    public int evolvesWithFriendship;
    public bool evolvesFromGender;
    public bool evolvesRandom;
}