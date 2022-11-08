using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveFunctions : MonoBehaviour
{
    public CombatScreen combatScreen;
    private const float WEAKNESS = 1.75f;
    private const float RESIST = 0.58f;

    public float GetTypeMatchup(StatLib.Type moveType, StatLib.Type defType1, StatLib.Type defType2){
        float ef1 = GetTypeEffectiveness(moveType, defType1);
        float ef2 = GetTypeEffectiveness(moveType, defType2);
        if(ef1 == WEAKNESS && ef2 == RESIST || ef1 == RESIST && ef2 == WEAKNESS){
            return 1f;
        }
        else{
            return ef1 * ef2;
        }
    }

    private float GetTypeEffectiveness(StatLib.Type offensiveType, StatLib.Type defensiveType){
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
                return WEAKNESS;
            case StatLib.Matchup.Resistance:
                return RESIST;
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
}
