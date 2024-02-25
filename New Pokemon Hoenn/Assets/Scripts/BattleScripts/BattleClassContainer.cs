using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//holds data relevant to one team that is reset when a battle starts
public class TeamBattleModifier
{
    public const int SPIKES_MAX_STACKS = 3;
    public const string ALLY_POS = "Ally's";
    public const string ENEMY_POS = "Foe's";
    public const string ALLY_PREFIX = "";
    public const string ENEMY_PREFIX = "Foe ";
    public const string WILD_PREFIX = "Wild ";
    public bool isPlayerTeam;
    public bool usedXItemThisBattle;
    public string teamPrefix;
    public string teamPossessive;
    public List<TeamDurationEffectInfo> teamEffects;
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
            teamPrefix = WILD_PREFIX;
        }
    }
} 

public class TeamDurationEffectInfo{
    public ApplyTeamDurationEffect effect;
    public int timer;

    public TeamDurationEffectInfo(ApplyTeamDurationEffect effect, int timer){
        this.effect = effect;
        this.timer = timer;
    }
}

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
    public ApplyTimedEffect timedEffect;
    public int timer;
    public BattleTarget target;

    public TimedEffectInfo(ApplyTimedEffect timedEffect, int timer, BattleTarget target){
        this.timedEffect = timedEffect;
        this.timer = timer;
        this.target = target;
    }
}

public class MultiTurnInfo{
    public MultiTurnEffect multiTurn;
    public GameObject useNext;
    public int forcedToUseUntilCounter;
    public bool recharging;

    public MultiTurnInfo(MultiTurnEffect multiTurn, int forcedToUseUntilCounter, bool recharging){
        this.multiTurn = multiTurn;
        this.forcedToUseUntilCounter = forcedToUseUntilCounter;
        this.recharging = recharging;
    }
}

//holds data relevant to a single pokemon that is reset on tag-in
public class IndividualBattleModifier
{
    public const int MAX_STAT_STAGES = 6;
    public const int MAX_ACCURACY_STAGES = 5;
    public const int MAX_CRIT_STAGES = 10;
    public const int MAX_STOCKPILE_COUNT = 3;
    public List<AppliedEffectInfo> inflictingEffects;
    public List<AppliedEffectInfo> appliedEffects;
    public List<TimedEffectInfo> timedEffects; //timed effects are not overwritten on switch
    public MultiTurnInfo multiTurnInfo;
    public List<BattleTarget> targets;
    public int[] statStages;
    public float[] statMultipliers;
    public int physicalDamageTakenThisTurn;
    public int specialDamageTakenThisTurn;
    public int bideDamage;
    public BattleTarget lastOneToDealDamage;
    public PokemonType chargedType; //currently charge is the only move that affects this
    public Pokemon switchingIn;
    public int stockpileCount;
    public SemiInvulnerable semiInvulnerable;
    public GameObject disabledMove;
    public int consecutiveMoveCounter;
    public int protectCounter;
    public GameObject mimicMove;
    public int mimicPP;
    public bool flinched;
    public int toxicCounter; //resets when switching out

    public IndividualBattleModifier(List<TimedEffectInfo> timedEffects){
        inflictingEffects = new List<AppliedEffectInfo>();
        appliedEffects = new List<AppliedEffectInfo>();
        this.timedEffects = timedEffects == null ? new List<TimedEffectInfo>() : timedEffects;
        targets = new List<BattleTarget>();
        statStages = new int[8];
        statMultipliers = new float[5]{1,1,1,1,1};
    }

    public IndividualBattleModifier(IndividualBattleModifier oldModifier, List<TimedEffectInfo> timedEffects) : this(timedEffects){
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyConfuse>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyLockOn>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyTrap>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyLeechSeed>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyCurse>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyIngrain>());
        appliedEffects.Add(oldModifier.GetEffectInfoOfType<ApplyPerishSong>());
        appliedEffects.RemoveAll(effectInfo => effectInfo is null);
        statStages = oldModifier.statStages;
        CalculateStatMultipliers();
    }

    public AppliedEffectInfo GetEffectInfoOfType<T>(bool inflicting = false) where T : ApplyIndividualEffect {
        List<AppliedEffectInfo> effectInfoCollection = inflicting ? inflictingEffects : appliedEffects;
        return effectInfoCollection.Find(e => e.effect is T);
    }

    public void CalculateStatMultipliers(){
        for(int i = 0; i < 5; i++){
            statMultipliers[i] = ((float)Mathf.Abs(statStages[i]) + 3f) / 3f;
            if(statStages[i] < 0){
                statMultipliers[i] = 1f / statMultipliers[i];
            }
        }
    }
}

public class BattleTarget
{
    public Pokemon pokemon;
    public TeamBattleModifier teamBattleModifier;
    public IndividualBattleModifier individualBattleModifier;
    public BattleHUD battleHUD;
    public GameObject monSpriteObject;
    public Image monInnerMask;
    public GameObject turnAction;

    public BattleTarget(TeamBattleModifier tbm, IndividualBattleModifier ibm, Pokemon pokemon, BattleHUD hud, GameObject monSpriteObject){
        this.teamBattleModifier = tbm;
        this.individualBattleModifier = ibm;
        this.pokemon = pokemon;
        this.battleHUD = hud;
        this.monSpriteObject = monSpriteObject;
        monInnerMask = monSpriteObject.transform.GetChild(0).GetComponent<Image>();
    }

    public string GetName(){
        return teamBattleModifier.teamPrefix + pokemon.nickName;
    }

    public bool IsActive(){
        return pokemon != null && pokemon.primaryStatus != PrimaryStatus.Fainted;
    }
}

