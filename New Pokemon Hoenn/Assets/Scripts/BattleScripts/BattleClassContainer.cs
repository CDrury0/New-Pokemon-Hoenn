using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//holds data relevant to one team that is reset when a battle starts
public class TeamBattleModifier
{
    public const string ALLY_POS = "Ally's";
    public const string ENEMY_POS = "Foe's";
    public const string ALLY_PREFIX = "";
    public const string ENEMY_PREFIX = "Foe ";
    public const string WILD_PREFIX = "Wild ";
    public bool isPlayerTeam;
    public string teamPrefix;
    public string teamPossessive;
    public List<TeamDurationEffectInfo> teamEffects;
    public StatLib.Type sportAgainst;
    public int spikesCount;

    public TeamBattleModifier(bool isTrainerBattle, bool isPlayerTeam){
        
        teamEffects = new List<TeamDurationEffectInfo>();
        this.isPlayerTeam = isPlayerTeam;

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
    public ApplyIndividualEffect effect;
    public int timer;
    public BattleTarget inflictor;

    public AppliedEffectInfo(ApplyIndividualEffect effect, int timer, BattleTarget inflictor){
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
    public List<BattleTarget> targets;
    public OnFaintEffect onFaintEffect; //destiny bond or grudge
    public BattleTarget lastAttacker;
    public int[] statStages;
    public float[] statMultipliers;
    public int physicalDamageTakenThisTurn;
    public int specialDamageTakenThisTurn;
    public int bideDamage;
    public StatLib.Type chargedType; //currently charge is the only move that affects this
    public Pokemon switchingIn;
    public bool recharging;
    public int stockpileCount;
    public SemiInvulnerable semiInvulnerable;
    public GameObject lastUsedMove;
    public int consecutiveMoveCounter; //how many times a move has been used consecutively
    public int forcedToUseUntilCounter; //the number of times the move must be used to allow selection of a new action (e.g. 2 or 3 at random for thrash)
    public GameObject mimicMove;
    public int mimicPP;
    public bool flinched;

    public void CalculateStatMultipliers(){
        for(int i = 0; i < 5; i++)
        {
            statMultipliers[i] = (float)(Mathf.Abs(statStages[i]) + 3f) / 3f;
            if(statStages[i] < 0)
            {
                statMultipliers[i] = 1f / statMultipliers[i];
            }
        }
    }

    public IndividualBattleModifier(){
        inflictingTetherEffects = new List<AppliedEffectInfo>();
        affectedTetherEffects = new List<AppliedEffectInfo>();
        appliedIndividualEffects = new List<AppliedEffectInfo>();
        timedEffects = new List<TimedEffectInfo>();
        movesLastUsedAgainstThis = new List<GameObject>();
        movesBlockedByImprison = new List<GameObject>();
        targets = new List<BattleTarget>();
        statStages = new int[8];
        statMultipliers = new float[5]{1,1,1,1,1};
    }

    //add overloaded constructor to account for passed effects via baton pass
}

public class BattleTarget
{
    [HideInInspector] public Pokemon pokemon;
    public TeamBattleModifier teamBattleModifier;
    public IndividualBattleModifier individualBattleModifier;
    public BattleHUD battleHUD;
    public GameObject monSpriteObject;
    [HideInInspector] public GameObject turnAction;

    public BattleTarget(TeamBattleModifier tbm, IndividualBattleModifier ibm, Pokemon pokemon, BattleHUD hud, GameObject monSpriteObject){
        this.teamBattleModifier = tbm;
        this.individualBattleModifier = ibm;
        this.pokemon = pokemon;
        this.battleHUD = hud;
        this.monSpriteObject = monSpriteObject;
    }

    public string GetName(){
        return teamBattleModifier.teamPrefix + pokemon.nickName;
    }
}

