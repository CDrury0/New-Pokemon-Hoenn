using UnityEngine;
using System.Collections.Generic;

public enum PrimaryStatus {None, Poisoned, Burned, Paralyzed, Asleep, Frozen, Fainted, Any}
public enum Gender {None, Male, Female}

public class Pokemon
{
    public const int MAX_LEVEL = 100;
    public const int MAX_EV = 252;
    public const int MAX_EV_TOTAL = 512;
    public const int MAX_IV = 31;
    public const int MAX_FRIENDSHIP = 255;
    public PokemonDefault pokemonDefault;
    public List<GameObject> moves;
    public int[] movePP = new int[4];
    public int[] moveMaxPP = new int[4];
    public int[] effortValues = new int[6];
    public int[] individualValues = new int[6];
    public PokemonType hiddenPowerType;
    public PrimaryStatus primaryStatus;
    public PokemonType type1;
    public PokemonType type2;
    public bool isShiny;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite boxSprite;
    public ItemData heldItem;

    /// <summary>
    /// Update property implementation to account for met location, luxury ball, etc?
    /// </summary>
    public int Friendship
    {
        get { return _friendship; }
        set
        {
            if (value > MAX_FRIENDSHIP)
            {
                _friendship = MAX_FRIENDSHIP;
            }
            else if (value < 0)
            {
                _friendship = 0;
            }
            else
            {
                _friendship = value;
            }
        }
    }
    private int _friendship;
    public int level;
    public int experience;
    public float height;
    public float weight;
    public string pokemonName;
    public string nickName;
    public PokemonNature nature;
    public Gender gender;
    public int abilitySlot;
    public StatLib.Ability ability;
    public ItemData ballUsed;
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

    //used in the real game for wild pokemon (constructor is called from GenerationValues)
    public Pokemon(PokemonDefault reference, int level, bool learnMoves = true){
        FillValues(reference, level);
        if(learnMoves){
            WildPokemonLearnMoves(reference, level);
        }
    }

    /// <summary>
    /// Used to create a usable copy of a template from a trainer party, etc.
    /// </summary>
    public Pokemon(SerializablePokemon p){
        this.pokemonDefault = p.pokemonDefault;
        this.ability = p.ability;
        this.isShiny = p.isShiny;
        FillSprites(this);
        this.ballUsed = p.ballUsed;
        this.effortValues = p.effortValues;
        this.individualValues = p.individualValues;
        this.level = p.level;
        this.nature = p.nature ?? ReferenceLib.GetNatures().Find(n => n.name == "Hardy");
        UpdateStats();
        this.CurrentHealth = stats[0];
        this.Friendship = p.friendship;
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

    public void UpdateStats(){
        int[] newStats = new int[6];

        if(pokemonDefault.baseStats[0] == 1){
            newStats[0] = 1;
        }
        else{
            float hp = pokemonDefault.baseStats[0] * 2;
            hp += individualValues[0];
            hp += effortValues[0] / 4;
            hp *= level;
            hp /= 100;
            hp += level + 10;
            newStats[0] = (int)hp;
        }

        float[] natureMultiplier = nature.GetNatureModifiers();

        for(int i = 1; i < newStats.Length; i++){
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
        if(primaryStatus != PrimaryStatus.Fainted){
            float hpRatio = CurrentHealth / (float)oldMaxHp;
            CurrentHealth = (int)(hpRatio * stats[0]);
        }
    }

    public void FillPP(int whichMove){
        MoveData move = this.moves[whichMove].GetComponent<MoveData>();
        moveMaxPP[whichMove] = move.maxPP;
        movePP[whichMove] = move.maxPP;
    }

    private void FillSprites(Pokemon p){
        if (p.isShiny){
            p.frontSprite = p.pokemonDefault.shinyFront;
            p.backSprite = p.pokemonDefault.shinyBack;
            p.boxSprite = p.pokemonDefault.shinyBoxSprite;
        }
        else{
            p.frontSprite = p.pokemonDefault.normalFront;
            p.backSprite = p.pokemonDefault.normalBack;
            p.boxSprite = p.pokemonDefault.boxSprite;
        }
    }

    public bool IsThisType(PokemonType type){
        return type1 == type || type2 == type;
    }

    public PokemonNature GetRandomNature(){
        List<PokemonNature> natures = ReferenceLib.GetNatures();
        return natures[Random.Range(0, natures.Count)];
    }

    private void FillValues(PokemonDefault pokemonDefault, int level){
        this.pokemonDefault = pokemonDefault;
        this.level = level;
        effortValues = new int[] { 0, 0, 0, 0, 0, 0 };
        individualValues = MakeRandomIVS();
        isShiny = Random.Range(0, 1) == 0;
        FillSprites(this);
        hiddenPowerType = ReferenceLib.Instance.typeList[Random.Range(0, 18)];
        Friendship = pokemonDefault.friendship;
        experience = pokemonDefault.CalculateExperienceAtLevel(level);
        height = MakeHeight(pokemonDefault);
        weight = MakeWeight(pokemonDefault);
        pokemonName = pokemonDefault.pokemonName;
        nature = GetRandomNature();
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

    private void WildPokemonLearnMoves(PokemonDefault pokemonDefault, int level){
        for(int i = level; i >= 0; i--){
            if (!moves.Contains(null)){
                break;
            }
            if (pokemonDefault.learnedMoves[i] != null){
                int replaced = moves.IndexOf(null);
                moves[replaced] = pokemonDefault.learnedMoves[i];
                moveMaxPP[replaced] = pokemonDefault.learnedMoves[i].GetComponent<MoveData>().maxPP;
                movePP[replaced] = pokemonDefault.learnedMoves[i].GetComponent<MoveData>().maxPP;
            }
        }
    }

    private ItemData MakeHeldItem(ItemData heldItem){
        return heldItem != null && Random.Range(0, 10) == 0 ? heldItem : null;
    }

    private Gender MakeGender(float ratio){
        if (ratio < 0f){
            return Gender.None;
        }
        else if (Random.Range(0f, 1f) <= ratio){
            return Gender.Male;
        }
        else{
            return Gender.Female;
        }
    }

    private float MakeHeight(PokemonDefault pokemonDefault){
        float rawHeight = pokemonDefault.height * Random.Range(0.8f, 1.2f);
        return (float)System.Math.Round(rawHeight, 2);
    }

    private float UpdateHeight(PokemonDefault pokemonDefault){
        float heightRatio = height / this.pokemonDefault.height;
        return (float)System.Math.Round(heightRatio * pokemonDefault.height, 2);
    }

    private float MakeWeight(PokemonDefault pokemonDefault){
        float rawWeight = pokemonDefault.weight * Random.Range(0.75f, 1.25f);
        return (float)System.Math.Round(rawWeight, 2);
    }

    private float UpdateWeight(PokemonDefault pokemonDefault){
        float weightRatio = weight / this.pokemonDefault.weight;
        return (float)System.Math.Round(weightRatio * pokemonDefault.weight, 2);
    }

    private int[] MakeRandomIVS(){
        int[] ivs = new int[6];
        for (int i = 0; i < ivs.Length; i++){
            ivs[i] = Random.Range(0, MAX_IV + 1);
        }
        return ivs;
    }
}