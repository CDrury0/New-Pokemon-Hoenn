using UnityEngine;
using System.Collections.Generic;

//holds data relevant to one team that is reset when a battle starts
public class TeamBattleModifier
{
    public const string ALLY_POS = "Ally's ";
    public const string ENEMY_POS = "Foe's ";
    public const string ALLY_PREFIX = "";
    public const string ENEMY_PREFIX = "Foe ";
    public const string WILD_PREFIX = "Wild ";
    public string teamPrefix;
    public string teamPossessive;
    public List<TeamDurationEffectInfo> teamEffects;
    public StatLib.Type sportAgainst;
    public int spikesCount;

    public TeamBattleModifier(bool isTrainerBattle, bool isPlayerTeam){
        
        teamEffects = new List<TeamDurationEffectInfo>();

        if(isPlayerTeam){
            teamPossessive = ALLY_POS;
            teamPrefix = ALLY_PREFIX;
        }
        else if(isTrainerBattle){
            teamPossessive = ENEMY_POS;
            teamPrefix = ENEMY_PREFIX;
        }
        else{
            teamPossessive = ENEMY_POS;
            teamPrefix = ENEMY_PREFIX;
        }
    }
} 

public class TeamDurationEffectInfo{
    public TeamDurationEffect effect;
    public int timer;

    public TeamDurationEffectInfo(TeamDurationEffect effect, int timer){
        this.effect = effect;
        this.timer = timer;
    }
}

//if the target.individualmodifier contains a tethereffect and inflictor.individual.inflictingTetherEffects doesn't, release at end of turn
public class AppliedEffectInfo{
    public ApplyEffect effect;
    public int timer;
    public BattleTarget inflictor;

    public AppliedEffectInfo(ApplyEffect effect, int timer, BattleTarget inflictor){
        this.effect = effect;
        this.timer = timer;
        this.inflictor = inflictor;
    }
}

public class TimedEffectInfo{
    public GameObject timedEffect;
    public int timer;
    public BattleTarget inflictor;

    public TimedEffectInfo(GameObject timedEffect, int timer, BattleTarget inflictor){
        this.timedEffect = timedEffect;
        this.timer = timer;
        this.inflictor = inflictor;
    }
}

//holds data relevant to a single pokemon that is reset on tag-in
public class IndividualBattleModifier
{
    public const int MAX_STAT_STAGES = 6;
    public const int MAX_ACCURACY_STAGES = 5;
    public List<AppliedEffectInfo> inflictingTetherEffects;
    public List<AppliedEffectInfo> affectedTetherEffects;
    public List<AppliedEffectInfo> appliedIndividualEffects;
    public List<TimedEffectInfo> timedEffects; //timed effects are not overwritten on switch
    public List<GameObject> movesLastUsedAgainstThis;
    public List<GameObject> movesBlockedByImprison;
    public ApplyEffect onFaintEffect; //destiny bond or grudge
    public BattleTarget lastAttacker;
    public int[] statStages;
    public int physicalDamageTakenThisTurn;
    public int specialDamageTakenThisTurn;
    public int bideDamage;
    public StatLib.Type chargedType; //currently charge is the only move that affects this
    public Pokemon switchingIn;
    public bool recharging;
    public int stockpileCount;
    public GameObject lastUsedMove;
    public int consecutiveMoveCounter; //how many times a move has been used consecutively
    public int forcedToUseUntilCounter; //the number of times the move must be used to allow selection of a new action (e.g. 2 or 3 at random for thrash)
    public GameObject mimicMove;
    public int mimicPP;


    public IndividualBattleModifier(){
        inflictingTetherEffects = new List<AppliedEffectInfo>();
        affectedTetherEffects = new List<AppliedEffectInfo>();
        appliedIndividualEffects = new List<AppliedEffectInfo>();
        timedEffects = new List<TimedEffectInfo>();
        movesLastUsedAgainstThis = new List<GameObject>();
        movesBlockedByImprison = new List<GameObject>();
        statStages = new int[8];
    }

    //add overloaded constructor to account for passed effects via baton pass
}

//holds UI objects that display data for 1 pokemon
[System.Serializable]
public class BattleHUD
{
    public HealthBar hpBar;
}

[System.Serializable]
public class BattleTarget
{
    [HideInInspector] public Pokemon pokemon;
    public TeamBattleModifier teamBattleModifier;
    public IndividualBattleModifier individualBattleModifier;
    public BattleHUD battleHUD;
    [HideInInspector] public GameObject turnAction;

    public BattleTarget(TeamBattleModifier tbm, IndividualBattleModifier ibm, Pokemon pokemon){
        this.teamBattleModifier = tbm;
        this.individualBattleModifier = ibm;
        this.pokemon = pokemon;
        this.pokemon.inBattle = true;
    }
}

