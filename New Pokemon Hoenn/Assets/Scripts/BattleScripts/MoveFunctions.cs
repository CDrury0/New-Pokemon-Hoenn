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
        float ef1 = GetSingleTypeEffectiveness(moveType, defType1);
        float ef2 = GetSingleTypeEffectiveness(moveType, defType2);
        if(ef1 == TYPE_WEAKNESS && ef2 == TYPE_RESIST || ef1 == TYPE_RESIST && ef2 == TYPE_WEAKNESS){
            return 1f;
        }
        else{
            return ef1 * ef2;
        }
    }

    private float GetSingleTypeEffectiveness(StatLib.Type offensiveType, StatLib.Type defensiveType){
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

    public StatLib.Type GetMoveTypeFromWeather(Weather weather){
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
    
    public bool MustChooseTarget(TargetType targetType, BattleTarget user, List<BattleTarget> battleTargets, bool doubleBattle){
        if(targetType == TargetType.Self){
            user.individualBattleModifier.targets = new List<BattleTarget>(){user};
            return false;
        }
        if(!doubleBattle){
            if(targetType == TargetType.Ally){
                user.individualBattleModifier.targets = new List<BattleTarget>(){null};
            }
            else{
                user.individualBattleModifier.targets = new List<BattleTarget>(){battleTargets.First(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam)};
            }
            return false;
        }
        List<BattleTarget> moveTargets = battleTargets.Where(b => b != user).ToList();
        if(targetType == TargetType.Ally){
            user.individualBattleModifier.targets = new List<BattleTarget>(){moveTargets.FirstOrDefault(b => b.teamBattleModifier.isPlayerTeam == user.teamBattleModifier.isPlayerTeam)};
            return false;
        }
        if(targetType == TargetType.Foes){
            user.individualBattleModifier.targets = new List<BattleTarget>(moveTargets.Where(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam).ToList());
            return false;
        }
        if(targetType == TargetType.RandomFoe){
            List<BattleTarget> enemies = moveTargets.Where(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam).ToList();
            user.individualBattleModifier.targets = new List<BattleTarget>(){enemies[Random.Range(0, enemies.Count)]};
            return false;
        }
        if(targetType == TargetType.All){
            user.individualBattleModifier.targets = new List<BattleTarget>(moveTargets);
            return false;
        }
        if(targetType == TargetType.Single){    //add conditions for encore, follow me, etc.
            if(moveTargets.Count > 1){
                return true;
            }
            else{
                user.individualBattleModifier.targets = new List<BattleTarget>(){moveTargets[0]};
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
            if(battleTargets[i].turnAction.GetComponent<MoveData>().pursuit && battleTargets[i].individualBattleModifier.targets[0].turnAction.CompareTag("Switch")){
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

    public IEnumerator WriteEffectivenessText(BattleTarget target, StatLib.Type effectiveMoveType){
        float matchup = GetTypeMatchup(effectiveMoveType, target.pokemon.type1, target.pokemon.type2);
        if(matchup > 1){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It's super effective!"));
        }
        else if(matchup < 1){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It's not very effective..."));
        }
    }

    public IEnumerator CheckMoveFailedToBeUsed(CombatSystem.WrappedBool moveFailed, BattleTarget user){
        moveFailed.failed = true;
        if(user.pokemon.primaryStatus == PrimaryStatus.Paralyzed && Random.Range(0f, 1f) < 0.2f){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is paralyzed! It can't move!"));
            yield break;
        }
        else if(user.pokemon.primaryStatus == PrimaryStatus.Asleep){
            if(user.pokemon.sleepCounter > 0){
                user.pokemon.sleepCounter--;
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is fast asleep"));
                yield break;
            }
            else{
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " woke up!"));
                user.pokemon.primaryStatus = PrimaryStatus.None;
            }
        }
        else if(user.pokemon.primaryStatus == PrimaryStatus.Frozen){
            if(Random.Range(0f, 1f) < 0.25f){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " thawed out!"));
                user.pokemon.primaryStatus = PrimaryStatus.None;
            }
            else{
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is frozen solid!"));
                yield break;
            }
        }
        if(user.individualBattleModifier.flinched){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " flinched!"));
            yield break;
        }
        AppliedEffectInfo confuseEffect = user.individualBattleModifier.appliedIndividualEffects.FirstOrDefault(e => e.effect is ApplyConfuse);
        if(confuseEffect != null){
            if(confuseEffect.timer == 0){
                user.individualBattleModifier.appliedIndividualEffects.Remove(confuseEffect);
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " snapped out of confusion"));
            }
            else{
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is confused!"));
                if(Random.Range(0f, 1f) < 0.4f){
                    //self hit on confusion
                    yield break;
                }
            }

        }
        user.battleHUD.SetBattleHUD(user.pokemon);
        moveFailed.failed = false;
    }

    public IEnumerator CheckMoveFailedAgainstTarget(CombatSystem.WrappedBool moveFailed, GameObject turnAction, BattleTarget user, BattleTarget target){
        moveFailed.failed = true;
        MoveData moveData = turnAction.GetComponent<MoveData>();

        if(!moveData.accuracyData.CheckMoveHit(moveData, user, target, CombatSystem.Weather)){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + "'s attack missed"));
            yield break;
        }

        if(moveData.moveName == "Thunder Wave" && target.pokemon.IsThisType(StatLib.Type.Ground)){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It doesn't affect " + target.GetName() + "..."));
            yield break;
        }

        ICheckMoveFail[] componentsToCheck = turnAction.GetComponents<ICheckMoveFail>();
        foreach(ICheckMoveFail c in componentsToCheck){
            string failureMessage = c.CheckMoveFail(user, target, moveData);
            if(failureMessage != null){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(failureMessage));
                yield break;
            }
        }

        moveFailed.failed = false;
    }
}
