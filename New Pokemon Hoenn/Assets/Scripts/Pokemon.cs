using UnityEngine;
using System.Collections.Generic;

public enum PrimaryStatus {None, Poisoned, Burned, Paralyzed, Asleep, Frozen, Fainted, Any}
public enum Gender {None, Male, Female}
public enum Nature {Hardy, Lonely, Brave, Adamant, Naughty, Bold, Docile, Relaxed, Impish, Lax, Timid, Hasty, Serious, Jolly, Naive, Modest, Mild, Quiet, Rash, Calm, Gentle, Sassy, Careful}

public class Pokemon
{
    public enum Type {None, Normal, Fire, Water, Electric, Grass, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy}
    public PokemonDefault pokemonDefault;
    public List<GameObject> moves;
    public int[] movePP = new int[4];
    public int[] moveMaxPP = new int[4];
    public int[] effortValues = new int[6];
    public int[] individualValues = new int[6];
    public Pokemon.Type hiddenPowerType;
    public PrimaryStatus primaryStatus;
    public Pokemon.Type type1;
    public Pokemon.Type type2;
    public bool isShiny;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite boxSprite;
    public Item heldItem;
    public int friendship;
    public int level;
    public int experience;
    public float height;
    public float weight;
    public string pokemonName;
    public string nickName;
    public Nature nature;
    public Gender gender;
    public int abilitySlot;
    public StatLib.Ability ability;
    public Item ballUsed;
    public int metLevel;
    public string metArea;
    /// <summary>
    /// Stats are in the order: HP, Atk, Def, Sp. Atk, Sp. Def, Speed
    /// </summary>
    public int[] stats = new int[6];
    private int _currentHealth;
    public int CurrentHealth {
        get{return _currentHealth;}
        set{
            if(value < 0){
                _currentHealth = 0;
            }
            else if(value > stats[0]){
                _currentHealth = stats[0];
            }
            else{
                _currentHealth = value;
            }
        }}
    public int numberID;
    public int sleepCounter = 0;
    public bool toxic;
    public bool inBattle;

    //used in BattleTestMenu
    public Pokemon(PokemonDefault pokemonDefault, int level)
    {
        FillValues(pokemonDefault, level);
    }

    //used in the real game for wild pokemon (call constructor from spawner)
    public Pokemon(SpawnInfo[] activeSpawnInfo)
    {
        float rand = Random.Range(0f, 1f);
        float spawnRateCounter = 0f;
        for (int i = 0; i < activeSpawnInfo.Length; i++)
        {
            if (rand <= activeSpawnInfo[i].spawnRate + spawnRateCounter)
            {
                pokemonDefault = activeSpawnInfo[i].pokemonDefault;
                level = Random.Range(activeSpawnInfo[i].levelRange[0], activeSpawnInfo[i].levelRange[1] + 1);
                break;
            }
            spawnRateCounter += activeSpawnInfo[i].spawnRate;
        }

        FillValues(pokemonDefault, level);
        WildPokemonLearnMoves(pokemonDefault, level);
    }

    //create a usable copy of an existing pokemon from a trainer party
    public Pokemon(SerializablePokemon p){
        this.pokemonDefault = p.pokemonDefault;
        this.ability = p.ability;
        this.isShiny = p.isShiny;
        FillSprites(this);
        this.ballUsed = p.ballUsed;
        this.effortValues = p.effortValues;
        this.individualValues = p.individualValues;
        this.level = p.level;
        this.nature = p.nature;
        UpdateStats();
        this.CurrentHealth = stats[0];
        this.friendship = p.friendship;
        this.gender = p.gender;
        this.height = p.height;
        this.heldItem = p.heldItem;
        this.moves = new List<GameObject>(p.moves);
        this.moveMaxPP = new int[] { p.moveMaxPP[0], p.moveMaxPP[1], p.moveMaxPP[2], p.moveMaxPP[3] };
        this.movePP = new int[] {this.moveMaxPP[0], this.moveMaxPP[1], this.moveMaxPP[2], this.moveMaxPP[3]};
        this.nickName = p.pokemonDefault.pokemonName;
        this.primaryStatus = PrimaryStatus.None;
        this.type1 = this.pokemonDefault.type1;
        this.type2 = this.pokemonDefault.type2;
        this.weight = p.weight;
    }

    public void Evolve(PokemonDefault evolveInto){
        if(nickName == pokemonName){
            nickName = evolveInto.pokemonName;
        }
        pokemonName = evolveInto.pokemonName;
        height = UpdateHeight(evolveInto);
        weight = UpdateWeight(evolveInto);
        this.pokemonDefault = evolveInto;
        FillValues();
    }

    public void UpdateStats()
    {
        int[] newStats = new int[6];

        if(pokemonDefault.baseStats[0] == 1)
        {
            newStats[0] = 1;
        }
        else
        {
            float hp = pokemonDefault.baseStats[0] * 2;
            hp += individualValues[0];
            hp += effortValues[0] / 4;
            hp *= level;
            hp /= 100;
            hp += level + 10;
            newStats[0] = (int)hp;
        }

        float[] natureMultiplier = NatureMultiplier();

        for(int i = 1; i < newStats.Length; i++)
        {
            float stat = pokemonDefault.baseStats[i] * 2;
            stat += individualValues[i];
            stat += effortValues[i] / 4;
            stat *= level;
            stat /= 100;
            stat += 5;
            stat *= natureMultiplier[i];
            newStats[i] = (int)stat;
        }

        int oldMaxHp = stats[0];
        stats = newStats;
        if(primaryStatus != PrimaryStatus.Fainted)
        {
            float hpRatio = CurrentHealth / (float)oldMaxHp;
            CurrentHealth = (int)(hpRatio * stats[0]);
        }
    }

    public void FillPP(int whichMove){
        MoveData move = this.moves[whichMove].GetComponent<MoveData>();
        moveMaxPP[whichMove] = move.maxPP;
        movePP[whichMove] = move.maxPP;
    }

    private void FillSprites(Pokemon p)
    {
        if (p.isShiny)
        {
            p.frontSprite = p.pokemonDefault.shinyFront;
            p.backSprite = p.pokemonDefault.shinyBack;
            p.boxSprite = p.pokemonDefault.shinyBoxSprite;
        }
        else
        {
            p.frontSprite = p.pokemonDefault.normalFront;
            p.backSprite = p.pokemonDefault.normalBack;
            p.boxSprite = p.pokemonDefault.boxSprite;
        }
    }

    public bool IsThisType(Pokemon.Type type){
        return type1 == type || type2 == type;
    }

    public float[] NatureMultiplier()
    {
        float[] natureMultiplier = new float[6] { 1, 1, 1, 1, 1, 1 };

        switch (nature)
        {
            case Nature.Hardy:
                break;
            case Nature.Lonely:
                natureMultiplier[1] = 1.1f;
                natureMultiplier[2] = 0.9f;
                break;
            case Nature.Brave:
                natureMultiplier[1] = 1.1f;
                natureMultiplier[5] = 0.9f;
                break;
            case Nature.Adamant:
                natureMultiplier[1] = 1.1f;
                natureMultiplier[3] = 0.9f;
                break;
            case Nature.Naughty:
                natureMultiplier[1] = 1.1f;
                natureMultiplier[4] = 0.9f;
                break;
            case Nature.Bold:
                natureMultiplier[2] = 1.1f;
                natureMultiplier[1] = 0.9f;
                break;
            case Nature.Docile:
                break;
            case Nature.Relaxed:
                natureMultiplier[2] = 1.1f;
                natureMultiplier[5] = 0.9f;
                break;
            case Nature.Impish:
                natureMultiplier[2] = 1.1f;
                natureMultiplier[3] = 0.9f;
                break;
            case Nature.Lax:
                natureMultiplier[2] = 1.1f;
                natureMultiplier[4] = 0.9f;
                break;
            case Nature.Timid:
                natureMultiplier[5] = 1.1f;
                natureMultiplier[1] = 0.9f;
                break;
            case Nature.Hasty:
                natureMultiplier[5] = 1.1f;
                natureMultiplier[2] = 0.9f;
                break;
            case Nature.Serious:
                break;
            case Nature.Jolly:
                natureMultiplier[5] = 1.1f;
                natureMultiplier[3] = 0.9f;
                break;
            case Nature.Naive:
                natureMultiplier[5] = 1.1f;
                natureMultiplier[4] = 0.9f;
                break;
            case Nature.Modest:
                natureMultiplier[3] = 1.1f;
                natureMultiplier[1] = 0.9f;
                break;
            case Nature.Mild:
                natureMultiplier[3] = 1.1f;
                natureMultiplier[2] = 0.9f;
                break;
            case Nature.Quiet:
                natureMultiplier[3] = 1.1f;
                natureMultiplier[5] = 0.9f;
                break;
            case Nature.Rash:
                natureMultiplier[3] = 1.1f;
                natureMultiplier[4] = 0.9f;
                break;
            case Nature.Calm:
                natureMultiplier[4] = 1.1f;
                natureMultiplier[1] = 0.9f;
                break;
            case Nature.Gentle:
                natureMultiplier[4] = 1.1f;
                natureMultiplier[2] = 0.9f;
                break;
            case Nature.Sassy:
                natureMultiplier[4] = 1.1f;
                natureMultiplier[5] = 0.9f;
                break;
            case Nature.Careful:
                natureMultiplier[4] = 1.1f;
                natureMultiplier[3] = 0.9f;
                break;
        }

        return natureMultiplier;
    }

    private void FillValues(PokemonDefault pokemonDefault, int level)
    {
        this.pokemonDefault = pokemonDefault;
        this.level = level;
        effortValues = new int[] { 0, 0, 0, 0, 0, 0 };
        individualValues = MakeRandomIVS();
        isShiny = Random.Range(0, 1000) == 0;
        FillSprites(this);
        hiddenPowerType = (Pokemon.Type)Random.Range(1, 18);
        friendship = pokemonDefault.friendship;
        experience = pokemonDefault.CalculateExperienceAtLevel(level);
        height = MakeHeight(pokemonDefault);
        weight = MakeWeight(pokemonDefault);
        pokemonName = pokemonDefault.pokemonName;
        nature = (Nature)Random.Range(0, 23);
        gender = MakeGender(pokemonDefault.genderRatio);
        abilitySlot = Random.Range(1, 3);
        ability = abilitySlot == 1 ? pokemonDefault.ability1 : pokemonDefault.ability2;
        UpdateStats();
        CurrentHealth = stats[0];
        heldItem = MakeHeldItem(pokemonDefault.naturallyHeldItem);
        nickName = pokemonName;
        metArea = ReferenceLib.Instance.activeArea.areaName;
        metLevel = level;
        type1 = pokemonDefault.type1;
        type2 = pokemonDefault.type2;
        numberID = Random.Range(100, 10000000);
        moves = new List<GameObject>{null, null, null, null}; //must put 4 nulls in to reserve the space so Capacity will report the correct value
    }

    //used upon evolution
    private void FillValues(){
        FillSprites(this);
        experience = pokemonDefault.CalculateExperienceAtLevel(level);
        ability = abilitySlot == 1 ? pokemonDefault.ability1 : pokemonDefault.ability2;
        UpdateStats();
        type1 = pokemonDefault.type1;
        type2 = pokemonDefault.type2;
    }

    private void WildPokemonLearnMoves(PokemonDefault pokemonDefault, int level)
    {
        for(int i = level; i >= 0; i--)
        {
            if (!moves.Contains(null))
            {
                break;
            }
            if (pokemonDefault.learnedMoves[i] != null)
            {
                int replaced = moves.IndexOf(null);
                moves[replaced] = pokemonDefault.learnedMoves[i];
                moveMaxPP[replaced] = pokemonDefault.learnedMoves[i].GetComponent<MoveData>().maxPP;
                movePP[replaced] = pokemonDefault.learnedMoves[i].GetComponent<MoveData>().maxPP;
            }
        }
    }

    private Item MakeHeldItem(Item heldItem)
    {
        return heldItem != null && Random.Range(0, 10) == 0 ? heldItem : null;
    }

    private Gender MakeGender(float ratio)
    {
        if (ratio < 0f)
        {
            return Gender.None;
        }
        else if (Random.Range(0f, 1f) <= ratio)
        {
            return Gender.Male;
        }
        else
        {
            return Gender.Female;
        }
    }

    private float MakeHeight(PokemonDefault pokemonDefault){
        float rawHeight = pokemonDefault.height * Random.Range(0.8f, 1.2f);
        return (float)System.Math.Round(rawHeight, 2);
    }

    private float UpdateHeight(PokemonDefault pokemonDefault){
        float heightRatio = height / this.pokemonDefault.height;
        return (float)(System.Math.Round(heightRatio * pokemonDefault.height, 2));
    }

    private float MakeWeight(PokemonDefault pokemonDefault){
        float rawWeight = pokemonDefault.weight * Random.Range(0.75f, 1.25f);
        return (float)System.Math.Round(rawWeight, 2);
    }

    private float UpdateWeight(PokemonDefault pokemonDefault){
        float weightRatio = weight / this.pokemonDefault.weight;
        return (float)(System.Math.Round(weightRatio * pokemonDefault.weight, 2));
    }

    private int[] MakeRandomIVS()
    {
        int[] ivs = new int[6];
        for (int i = 0; i < ivs.Length; i++)
        {
            ivs[i] = Random.Range(0, 32);
        }
        return ivs;
    }
}