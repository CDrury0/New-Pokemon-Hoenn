using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveFunctions : MonoBehaviour
{
    public CombatScreen combatScreen;
    private const float TYPE_WEAKNESS = 1.75f;
    private const float TYPE_RESIST = 0.58f;

    public float GetTypeMatchup(StatLib.Type moveType, StatLib.Type defType1, StatLib.Type defType2){
        float ef1 = GetTypeEffectivenessPrivate(moveType, defType1);
        float ef2 = GetTypeEffectivenessPrivate(moveType, defType2);
        if(ef1 == TYPE_WEAKNESS && ef2 == TYPE_RESIST || ef1 == TYPE_RESIST && ef2 == TYPE_WEAKNESS){
            return 1f;
        }
        else{
            return ef1 * ef2;
        }
    }

    private float GetTypeEffectivenessPrivate(StatLib.Type offensiveType, StatLib.Type defensiveType){
        if(offensiveType == StatLib.Type.None){
            return 1;
        }
        ReferenceLib.TypeMatchupList matchupList = ReferenceLib.Instance.typeEffectivenessMatchups.First(type => type.attackingType == offensiveType);
        ReferenceLib.TypeMatchupValues values = matchupList.matchup.FirstOrDefault(type => type.defendingType == defensiveType);
        return values != null ? MatchTypeEffectivenessToValue(values.effectiveness) : 1;
    }

    private float MatchTypeEffectivenessToValue(StatLib.Matchup matchup){
        switch (matchup){
            case StatLib.Matchup.Weakness:
                return TYPE_WEAKNESS;
            case StatLib.Matchup.Resistance:
                return TYPE_RESIST;
            default:
                return 0;
        }
    }
    
    public bool MustChooseTarget(TargetType targetType, BattleTarget target, List<BattleTarget> battleTargets, bool doubleBattle){
        if(targetType == TargetType.Self){
            target.individualBattleModifier.targets = new List<BattleTarget>(){target};
            return false;
        }
        if(!doubleBattle){
            if(targetType == TargetType.Ally){
                target.individualBattleModifier.targets = new List<BattleTarget>(){null};
            }
            else{
                target.individualBattleModifier.targets = new List<BattleTarget>(){battleTargets.First(b => b.teamBattleModifier.isPlayerTeam != target.teamBattleModifier.isPlayerTeam)};
            }
            return false;
        }
        List<BattleTarget> moveTargets = battleTargets.Where(b => b != target).ToList();
        if(targetType == TargetType.Ally){
            target.individualBattleModifier.targets = new List<BattleTarget>(){moveTargets.FirstOrDefault(b => b.teamBattleModifier.isPlayerTeam == target.teamBattleModifier.isPlayerTeam)};
            return false;
        }
        if(targetType == TargetType.Foes){
            target.individualBattleModifier.targets = new List<BattleTarget>(moveTargets.Where(b => b.teamBattleModifier.isPlayerTeam != target.teamBattleModifier.isPlayerTeam).ToList());
            return false;
        }
        if(targetType == TargetType.RandomFoe){
            List<BattleTarget> enemies = moveTargets.Where(b => b.teamBattleModifier.isPlayerTeam != target.teamBattleModifier.isPlayerTeam).ToList();
            target.individualBattleModifier.targets = new List<BattleTarget>(){enemies[Random.Range(0, enemies.Count)]};
            return false;
        }
        if(targetType == TargetType.All){
            target.individualBattleModifier.targets = new List<BattleTarget>(moveTargets);
            return false;
        }
        if(targetType == TargetType.Single){    //add conditions for encore, follow me, etc.
            if(moveTargets.Count > 1){
                return true;
            }
            else{
                target.individualBattleModifier.targets = new List<BattleTarget>(){moveTargets[0]};
                return false;
            }
        }
        return false;
    }

    //account for quick claw later
    public List<int> GetTurnOrder(List<BattleTarget> battleTargets){
        List<int> order = new List<int>(battleTargets.Count);
        order.Add(0);
        int[] priorities = new int[battleTargets.Count];
        for(int i = 0; i < battleTargets.Count; i++){
            if(battleTargets[i].turnAction.GetComponent<MoveData>().pursuit && battleTargets[i].individualBattleModifier.targets[0].turnAction.GetComponent<MoveData>().priority == 6){
                priorities[i] = 7;
            }
            else{
                priorities[i] = battleTargets[i].turnAction.GetComponent<MoveData>().priority;
            }
        }
        for(int i = 1; i < battleTargets.Count; i++){
            for(int j = 0; j < order.Count; j++){
                if(priorities[i] > priorities[order[j]]){
                    order.Insert(j, i);
                    break;
                }
                else if(priorities[i] < priorities[order[j]]){
                    continue;
                }
                else if(battleTargets[i].pokemon.stats[5] * battleTargets[i].individualBattleModifier.statMultipliers[4] > battleTargets[order[j]].pokemon.stats[5] * battleTargets[order[j]].individualBattleModifier.statMultipliers[4]){
                    order.Insert(j, i);
                    break;
                }
                else if(battleTargets[i].pokemon.stats[5] * battleTargets[i].individualBattleModifier.statMultipliers[4] == battleTargets[order[j]].pokemon.stats[5] * battleTargets[order[j]].individualBattleModifier.statMultipliers[4] && !battleTargets[i].teamBattleModifier.isPlayerTeam){
                    order.Insert(j, i);
                    break;
                }
            }
            if(!order.Contains(i)){
                order.Add(i);
            }
        }
        return order;
    }

    public bool RollCrit(BattleTarget user, bool highCritRate){
        int critStages = user.individualBattleModifier.statStages[7] + 1;
        critStages += highCritRate ? 1 : 0;
        //add to critStages for scope lens and other crit boosters
        float critRatio = 1f / (16f / (float)critStages);
        return Random.Range(0f, 0.99f) < critRatio;
    }

    public int NormalDamageFormula(int power, NormalDamage damageComponent, BattleTarget user, BattleTarget target, bool crit){
        MoveData moveData = user.turnAction.GetComponent<MoveData>();
        float workingDamage;
        float defenseRatio;
        float modifier = 1f;

        workingDamage = user.pokemon.level * 2;
        workingDamage /= 5;
        workingDamage += 2;
        workingDamage *= power;

        float offensiveMultiplier;
        float defensiveMultiplier;
        int offensiveStat;
        int defensiveStat;
        if(moveData.category == MoveData.Category.Physical){
            offensiveMultiplier = user.individualBattleModifier.statMultipliers[0];
            offensiveStat = user.pokemon.stats[1];
            defensiveMultiplier = target.individualBattleModifier.statMultipliers[1];
            defensiveStat = target.pokemon.stats[2];
        }
        else{
            offensiveMultiplier = user.individualBattleModifier.statMultipliers[2];
            offensiveStat = user.pokemon.stats[3];
            defensiveMultiplier = target.individualBattleModifier.statMultipliers[3];
            defensiveStat = target.pokemon.stats[4];
        }

        if(crit && offensiveMultiplier < 1f){
            offensiveMultiplier = 1f;
        }
        
        defenseRatio = (offensiveStat * offensiveMultiplier) / (defensiveStat * defensiveMultiplier);

        workingDamage *= defenseRatio;
        workingDamage /= 50;
        workingDamage += 2;

        StatLib.Type localType = moveData.typeFromWeather ? GetMoveTypeFromWeather(CombatSystem.Weather) : moveData.moveType;

        modifier *= GetTypeMatchup(localType, target.pokemon.type1, target.pokemon.type2);

        modifier *= localType == user.pokemon.type1 || moveData.moveType == user.pokemon.type2 ? 1.5f : 1f;

        modifier *= GetWeatherDamageModifier(localType, CombatSystem.Weather);

        if(user.individualBattleModifier.targets.Count > 1){
            modifier *= 0.75f;
        }

        //add conditions for guts
        if(user.pokemon.primaryStatus == PrimaryStatus.Burned && moveData.category == MoveData.Category.Physical && !damageComponent.facade){
            modifier *= 0.5f;
        }

        //modifiers like minimize, etc.
        if(damageComponent.bonusAgainstMinimize && target.individualBattleModifier.appliedIndividualEffects.OfType<ApplyMinimize>().Any()){
            modifier *= 2f;
        }

        if(damageComponent.bonusAgainstSemiInvulnerable != SemiInvulnerable.None && damageComponent.bonusAgainstSemiInvulnerable == target.individualBattleModifier.semiInvulnerable){
            modifier *= 2f;
        }

        if(moveData.category == MoveData.Category.Physical && target.teamBattleModifier.teamEffects.FirstOrDefault(e => e.effect == TeamDurationEffect.Reflect) != null && moveData.gameObject.GetComponent<BreaksWalls>() == null){
            modifier *= 0.5f;
        }

        if(moveData.category == MoveData.Category.Special && target.teamBattleModifier.teamEffects.FirstOrDefault(e => e.effect == TeamDurationEffect.LightScreen) != null && moveData.gameObject.GetComponent<BreaksWalls>() == null){
            modifier *= 0.5f;
        }

        if(crit){
            modifier *= 1.5f;
        }

        workingDamage *= modifier;
        workingDamage *= Random.Range(0.9f, 1f);
        return workingDamage > 1 ? (int)workingDamage : 1;
    }

    private float GetWeatherDamageModifier(StatLib.Type moveType, Weather weather){
        const float WEATHER_BONUS = 1.4f;
        const float WEATHER_PENALTY = 0.6f;
        if(weather == Weather.Rain){
            if(moveType == StatLib.Type.Water){
                return WEATHER_BONUS;
            }
            else if(moveType == StatLib.Type.Fire){
                return WEATHER_PENALTY;
            }
        }
        else if(weather == Weather.Sunlight){
            if(moveType == StatLib.Type.Water){
                return WEATHER_PENALTY;
            }
            else if(moveType == StatLib.Type.Fire){
                return WEATHER_BONUS;
            }
        }
        return 1f;
    }

    private StatLib.Type GetMoveTypeFromWeather(Weather weather){
        switch(weather){
            case Weather.None:
            return StatLib.Type.Normal;
            case Weather.Hail:
            return StatLib.Type.Ice;
            case Weather.Rain:
            return StatLib.Type.Water;
            case Weather.Sunlight:
            return StatLib.Type.Fire;
            case Weather.Sandstorm:
            return StatLib.Type.Rock;
            default:
            Debug.Log("Weather bugged");
            return StatLib.Type.None;
        }
    }
}
