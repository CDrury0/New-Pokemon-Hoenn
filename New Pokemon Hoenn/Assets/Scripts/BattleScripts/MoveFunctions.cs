using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveFunctions : MonoBehaviour
{
    public CombatScreen combatScreen;
    public GameObject confuseAttack;
    public ApplyConfuse confuseAfterForcedToUse;
    public MoveData confuseAfterForcedToUseData;
    private const float TYPE_WEAKNESS = 1.75f;
    private const float TYPE_RESIST = 0.58f;

    public float GetTypeMatchup(StatLib.Type moveType, BattleTarget defender){
        if(defender.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyIdentify) != null){
            return 1f;
        }
        float ef1 = GetSingleTypeEffectiveness(moveType, defender.pokemon.type1);
        float ef2 = GetSingleTypeEffectiveness(moveType, defender.pokemon.type2);
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
    
    public bool MustChooseTarget(TargetType targetType, BattleTarget user){
        List<BattleTarget> battleTargets = new List<BattleTarget>(CombatLib.Instance.combatSystem.BattleTargets);
        if(targetType == TargetType.Self){
            user.individualBattleModifier.targets = new List<BattleTarget>(){user};
            return false;
        }
        if(!CombatLib.Instance.combatSystem.DoubleBattle){
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
        if(targetType == TargetType.Single){    //add conditions for encore, etc.
            if(moveTargets.Count > 1){
                return true;
            }
            user.individualBattleModifier.targets = new List<BattleTarget>(){moveTargets[0]};
            return false;
        }
        return false;
    }

    public bool LockedIntoAction(BattleTarget user){
        MultiTurnInfo multiTurnInfo = user.individualBattleModifier.multiTurnInfo;
        if(multiTurnInfo != null){
            if(multiTurnInfo.multiTurn.useNext != null){
                user.turnAction = multiTurnInfo.multiTurn.useNext;
            }
            return true;
        }
        return false;
    }

    //account for quick claw later
    public List<BattleTarget> GetTurnOrder(List<BattleTarget> battleTargets){
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
        List<BattleTarget> orderedTargets = new List<BattleTarget>();
        for(int i = 0; i < battleTargets.Count; i++){
            orderedTargets.Add(battleTargets[order[i]]);
        }
        return orderedTargets;
    }

    public bool RollCrit(BattleTarget user, bool highCritRate){
        int critStages = user.individualBattleModifier.statStages[7] + 1;
        critStages += highCritRate ? 1 : 0;
        //add to critStages for scope lens and other crit boosters
        float critRatio = 1f / (16f / (float)critStages);
        return Random.Range(0f, 0.99f) < critRatio;
    }

    public IEnumerator WriteEffectivenessText(BattleTarget target, StatLib.Type effectiveMoveType){
        float matchup = GetTypeMatchup(effectiveMoveType, target);
        if(matchup > 1){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It's super effective!"));
        }
        else if(matchup < 1){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It's not very effective..."));
        }
    }

    public bool IsChargingTurn(GameObject move){
        MultiTurnEffect multiTurn = move.GetComponent<MultiTurnEffect>();
        return multiTurn != null && multiTurn.chargingTurn;
    }

    public IEnumerator CheckMoveFailedToBeUsed(CombatSystem.WrappedBool moveFailed, BattleTarget user){
        moveFailed.failed = true;
        if(user.individualBattleModifier.multiTurnInfo != null && user.individualBattleModifier.multiTurnInfo.recharging){
            user.individualBattleModifier.multiTurnInfo = null;
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " must recharge!"));
            yield break;
        }
        if(user.pokemon.primaryStatus == PrimaryStatus.Paralyzed && Random.Range(0f, 1f) < 0.2f){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is paralyzed! It can't move!"));
            yield break;
        }
        else if(user.pokemon.primaryStatus == PrimaryStatus.Asleep){
            if(user.pokemon.sleepCounter > 0){
                user.pokemon.sleepCounter--;
                if(!user.turnAction.GetComponent<MoveData>().worksWhileAsleep){
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is fast asleep"));
                    yield break;
                }
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
        AppliedEffectInfo infatuationEffect = user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyInfatuate);
        if(infatuationEffect != null && user.individualBattleModifier.targets.Contains(infatuationEffect.inflictor)){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is in love with " + infatuationEffect.inflictor.GetName()));
            if(Random.Range(0, 2) == 0){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is immobilized by love!"));
                yield break;
            }
        }
        if(user.individualBattleModifier.flinched){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " flinched!"));
            yield break;
        }
        AppliedEffectInfo confuseEffect = user.individualBattleModifier.appliedEffects.FirstOrDefault(e => e.effect is ApplyConfuse);
        if(confuseEffect != null){
            if(confuseEffect.timer == 0){
                user.individualBattleModifier.appliedEffects.Remove(confuseEffect);
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " snapped out of confusion"));
            }
            else{
                confuseEffect.timer--;
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is confused!"));
                if(Random.Range(0f, 1f) < 0.4f){
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage("It hurt itself in its confusion!"));
                    GameObject action = Instantiate(confuseAttack);
                    foreach(MoveEffect e in action.GetComponents<MoveEffect>()){
                        yield return StartCoroutine(e.DoEffect(user, user, action.GetComponent<MoveData>()));
                    }
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
        
        if(IsChargingTurn(turnAction)){
            moveFailed.failed = false;
            yield break;
        }

        if(!moveData.accuracyData.CheckMoveHit(moveData, user, target)){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + "'s attack missed"));
            if(moveData.accuracyData.hurtsIfMiss > 0f){
                yield return StartCoroutine(ChangeTargetHealth(user, -(int)(user.pokemon.stats[0] * moveData.accuracyData.hurtsIfMiss)));
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " kept going and crashed!"));
            }
            yield break;
        }

        if(moveData.moveName == "Thunder Wave" && target.pokemon.IsThisType(StatLib.Type.Ground)){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("It doesn't affect " + target.GetName() + "..."));
            yield break;
        }

        //if any of these fail, the move fails altogether
        ICheckMoveFail[] componentsCausingMoveToFail = turnAction.GetComponents<ICheckMoveFail>();
        foreach(ICheckMoveFail c in componentsCausingMoveToFail){
            string failureMessage = c.CheckMoveFail(user, target, moveData);
            if(failureMessage != null){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(failureMessage));
                yield break;
            }
        }

        //if ALL of these fail, the move fails
        ICheckMoveEffectFail[] componentsCausingEffectToFail = turnAction.GetComponents<ICheckMoveEffectFail>();
        if(componentsCausingEffectToFail.Length > 0){
            int failCount = 0;
            foreach(ICheckMoveEffectFail effectThatMayFail in componentsCausingEffectToFail){
                if(effectThatMayFail.CheckMoveEffectFail(user, target, moveData)){
                    failCount++;
                }
            }
            if(failCount == componentsCausingEffectToFail.Length){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(MoveData.FAIL));
                yield break;
            }
        }

        moveFailed.failed = false;
    }

    public IEnumerator ChangeTargetHealth(BattleTarget target, int change){
        yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, change));
        target.pokemon.CurrentHealth += change;
    }

    public void DeductPP(BattleTarget user){
        //account for pressure
        if(user.pokemon.moves.Contains(user.turnAction)){
            user.pokemon.movePP[user.pokemon.moves.IndexOf(user.turnAction)]--;
        }
    }

    //TEST END OF TURN EFFECTS
    public IEnumerator EndOfTurnEffects(List<BattleTarget> battleTargets){
        //multi-turn resolution
        yield return StartCoroutine(HandleMultiTurn(battleTargets));

        //weather
        if(CombatSystem.Weather != Weather.None){
            yield return StartCoroutine(HandleWeather(battleTargets));
        }
        
        //bind effects
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyBind>(battleTargets));

        //curse
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyCurse>(battleTargets));

        //timed effects
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyTimedEffect>(battleTargets));

        //shed skin, rain dish, etc. (healing abilities)

        //held item healing

        //leech seed
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyIngrain>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyLeechSeed>(battleTargets));

        //primary status
        yield return StartCoroutine(HandleEndTurnStatus(battleTargets));

        //encore/taunt/disable check
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyEncore>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyDisable>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyTaunt>(battleTargets));

        //yawn
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyDrowsy>(battleTargets));

        //perish song
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyPerishSong>(battleTargets));

        //reflect/light screen/safeguard/mist

        //protect, endure, trap
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyTrap>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyProtect>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyEndure>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyMagicCoat>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplySnatch>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyInfatuate>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyHelpingHand>(battleTargets));

        ClearFlinch(battleTargets);

        //abilities that boost (speed boost)
    }

    private void ClearFlinch(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            b.individualBattleModifier.flinched = false;
        }
    }

    private IEnumerator DoAppliedEffectOfType <T> (List<BattleTarget> battleTargets) where T : IApplyEffect{
        for(int i = 0; i < battleTargets.Count; i++){
            List<AppliedEffectInfo> effectInfo = battleTargets[i].individualBattleModifier.appliedEffects.FindAll(e => e.effect is T);
            foreach(AppliedEffectInfo a in effectInfo){
                if(a != null){
                    IApplyEffect effect = (IApplyEffect)a.effect;
                    yield return StartCoroutine(effect.DoAppliedEffect(battleTargets[i], a));
                }
            }
        }
    }

    private IEnumerator HandleMultiTurn(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            if(b.individualBattleModifier.multiTurnInfo != null){
                yield return StartCoroutine(b.individualBattleModifier.multiTurnInfo.multiTurn.DoAppliedEffect(b, null));
            }
        }
    }

    private IEnumerator HandleWeather(List<BattleTarget> battleTargets){
        if(CombatSystem.weatherTimer == 0){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage("The " + CombatSystem.Weather + " subsided"));
            CombatSystem.Weather = Weather.None;
            yield break;
        }

        CombatSystem.weatherTimer--;
        yield return StartCoroutine(combatScreen.battleText.WriteMessage("The " + CombatSystem.Weather + " continues"));

        if(CombatSystem.Weather == Weather.Hail){
            foreach(BattleTarget b in battleTargets){
                if(!b.pokemon.IsThisType(StatLib.Type.Ice)){
                    int damage = (int)(0.0625f * b.pokemon.stats[0]);
                    yield return StartCoroutine(b.battleHUD.healthBar.SetHealthBar(b.pokemon, -damage));
                    b.pokemon.CurrentHealth -= damage;
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(b.GetName() + " is stricken by hail!"));
                }
            }
        }
        else if(CombatSystem.Weather == Weather.Sandstorm){
            foreach(BattleTarget b in battleTargets){
                if(!b.pokemon.IsThisType(StatLib.Type.Ground) && !b.pokemon.IsThisType(StatLib.Type.Rock) && !b.pokemon.IsThisType(StatLib.Type.Steel)){
                    int damage = (int)(0.0625f * b.pokemon.stats[0]);
                    yield return StartCoroutine(b.battleHUD.healthBar.SetHealthBar(b.pokemon, -damage));
                    b.pokemon.CurrentHealth -= damage;
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(b.GetName() + " is buffeted by the sandstorm!"));
                }
            }
        }
    }

    private IEnumerator HandleEndTurnStatus(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            if(b.pokemon.primaryStatus == PrimaryStatus.Poisoned){
                int poisonDamage;
                string poisonMessage;
                if(b.pokemon.toxic){
                    b.individualBattleModifier.toxicCounter++;
                    poisonDamage = (int)(0.0625f * b.individualBattleModifier.toxicCounter * b.pokemon.stats[0]);
                    poisonMessage = b.GetName() + " is badly poisoned!";
                }
                else{
                    poisonDamage = (int)(0.125f * b.pokemon.stats[0]);
                    poisonMessage = b.GetName() + " is hurt by poison";
                }
                yield return StartCoroutine(b.battleHUD.healthBar.SetHealthBar(b.pokemon, -poisonDamage));
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(poisonMessage));
                b.pokemon.CurrentHealth -= poisonDamage;
            }
            else if(b.pokemon.primaryStatus == PrimaryStatus.Burned){
                int burnDamage = (int)(0.1f * b.pokemon.stats[0]);
                yield return StartCoroutine(b.battleHUD.healthBar.SetHealthBar(b.pokemon, -burnDamage));
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(b.GetName() + " is hurt by its burn!"));
                b.pokemon.CurrentHealth -= burnDamage;
            }
        }
    }
}
