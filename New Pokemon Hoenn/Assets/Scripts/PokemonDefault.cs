using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthRate { Slow, MediumSlow, MediumFast, Fast }

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
    public string pokedexEntry;
    public StatLib.Ability ability1;
    public StatLib.Ability ability2;
    public Sprite normalFront;
    public Sprite normalBack;
    public Sprite shinyFront;
    public Sprite shinyBack;
    public EvoDetails evoDetails;
    public Sprite boxSprite;
    public Sprite shinyBoxSprite;
    public ItemData naturallyHeldItem;
    public AreaData lastSeen;
    public int[] evYield = new int[6];
    public GameObject[] learnedMoves;
    public GameObject[] eggMoves;
    public GameObject[] tutorMoves;

    public int CalculateExperienceAtLevel(int level) {
        return growthRate switch {
            GrowthRate.Fast => (int)(Mathf.Pow(level, 3) * 4 / 5),
            //level 1 mons have negative xp with this formula, so eggs should hatch at >= level 2 (this is an issue in the original game as well)
            GrowthRate.MediumSlow => (int)((((float)6 / 5) * Mathf.Pow(level, 3)) - (15 * Mathf.Pow(level, 2)) + (100 * level) - 140),
            GrowthRate.MediumFast => (int)Mathf.Pow(level, 3),
            GrowthRate.Slow => (int)(Mathf.Pow(level, 3) * 5 / 4),
            _ => 0,
        };
    }
}

[System.Serializable]
public class EvoDetails
{
    [Tooltip("0 will disallow evolution via level-up")]
    public int evolutionLevel;
    public ItemData evolvesWithHeldItem;
    public int evolvesWithFriendship;
    public bool evolvesFromGender;
    public bool evolvesRandom;
    [Tooltip("If evolution is gender-based or randomly determined, fill the relevant slots (fill this if there is only one form to evolve into)")]
    public PokemonDefault firstOrMale;
    [Tooltip("If evolution is gender-based or randomly determined, fill the relevant slots")]
    public PokemonDefault secondOrFemale;
    [Tooltip("Insert shedinja reference to indicate its use; leave null otherwise")]
    public PokemonDefault shedinja;
    [SerializeField] private List<EvoStoneEntry> stoneEvolutions;
    [System.Serializable] class EvoStoneEntry {
        public ItemData evoStone;
        public PokemonDefault evolution;
    }

    public PokemonDefault GetEvoStoneMatch(ItemData evoStone){
        return stoneEvolutions.Find(entry => entry.evoStone == evoStone).evolution;
    }

    public bool HasStoneEntry(ItemData evoStone){
        return stoneEvolutions.Find(entry => entry.evoStone == evoStone) != null;
    }
}