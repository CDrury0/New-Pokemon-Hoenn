using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveFunctions : MonoBehaviour
{
    public CombatScreen combatScreen;
    public CombatSystem combatSystem;
    public GameObject confuseAttack;
    public ApplyConfuse confuseAfterForcedToUse;
    public MoveData confuseAfterForcedToUseData;
    
    /// <summary>
    /// If manual target selection is not necessary, automatically set the target accordingly using the TargetType argument.
    /// </summary>
    /// <returns>Whether or not manual target selection is necessary</returns>
    public bool MustChooseTarget(TargetType targetType, BattleTarget user){
        List<BattleTarget> battleTargets = new List<BattleTarget>(CombatSystem.BattleTargets);
        if(targetType == TargetType.Self){
            user.individualBattleModifier.targets = new List<BattleTarget>(){user};
            return false;
        }
        if(!CombatLib.Instance.combatSystem.DoubleBattle){
            if(targetType == TargetType.Ally){
                user.individualBattleModifier.targets = new List<BattleTarget>(){null};
            }
            else{
                try{
                    user.individualBattleModifier.targets = new List<BattleTarget>(){battleTargets.First(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam)};
                }
                catch(System.InvalidOperationException e){
                    Debug.LogError(e);
                }
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
            if(multiTurnInfo.multiTurn.GetComponent<MoveData>().targetType == TargetType.RandomFoe){
                MustChooseTarget(TargetType.RandomFoe, user);
            }
            return true;
        }
        return false;
    }

    //account for quick claw later
    public List<BattleTarget> GetTurnOrder(List<BattleTarget> battleTargets){
        List<int> order = new(battleTargets.Count){ 0 };
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
        float critRatio = 1f / (24f / ((float)critStages * 2));
        return Random.Range(0f, 0.99f) < critRatio;
    }

    public IEnumerator WriteEffectivenessText(float matchup){
        string message = PokemonType.GetMatchupMessage(matchup);
        if(message != string.Empty){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(PokemonType.GetMatchupMessage(matchup)));
        }
    }

    public static bool IsChargingTurn(GameObject move){
        MultiTurnEffect multiTurn = move.GetComponent<MultiTurnEffect>();
        return multiTurn != null && multiTurn.chargingTurn;
    }

    public static List<GameObject> GetAllUnusableMoves(BattleTarget user){
        List<GameObject> unusableMoves = new List<GameObject>();
        foreach(AppliedEffectInfo effectInfo in user.individualBattleModifier.appliedEffects.FindAll(e => e.effect is ICheckMoveSelectable)){
            ICheckMoveSelectable effectProhibitingMoves = (ICheckMoveSelectable)effectInfo.effect;
            unusableMoves.AddRange(effectProhibitingMoves.GetUnusableMoves(user));
        }
        unusableMoves.Add(null);
        unusableMoves.AddRange(GetImprisonMoves(user));
        unusableMoves.AddRange(user.pokemon.moves.FindAll(move => user.pokemon.movePP[user.pokemon.moves.IndexOf(move)] == 0));
        return unusableMoves;
    }

    private static List<GameObject> GetImprisonMoves(BattleTarget user){
        List<BattleTarget> imprisonUsers = CombatSystem.BattleTargets.FindAll(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam && b.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyImprison) != null);
        List<GameObject> imprisonedMoves = new List<GameObject>();
        foreach(BattleTarget b in imprisonUsers){
            imprisonedMoves.AddRange(b.pokemon.moves);
        }
        return imprisonedMoves;
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
            if (Random.Range(0f, 1f) < 0.25f){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " thawed out!"));
                user.pokemon.primaryStatus = PrimaryStatus.None;
            }
            else{
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is frozen solid!"));
                yield break;
            }
        }
        AppliedEffectInfo infatuationEffect = user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyInfatuate);
        if(infatuationEffect != null){
            //cast the effect field to infatuate since the computer can't guarantee that on its own
            ApplyInfatuate infatuateComponent = infatuationEffect.effect as ApplyInfatuate;
            //remove the effect if the inflictor is no longer in the target slot
            //this needs to be done each time a move is attempted to be used
            infatuateComponent.RemoveIfInflictorSwitchedOut(user, infatuationEffect);
            //the value of infatuationEffect may refer to stale data if the effect was removed in the line above
            infatuationEffect = user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyInfatuate);
            if(infatuationEffect != null && user.individualBattleModifier.targets.Contains(infatuationEffect.inflictor)){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is in love with " + infatuationEffect.inflictor.GetName()));
                if(Random.Range(0, 2) == 0){
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " is immobilized by love!"));
                    yield break;
                }
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

        if(user.individualBattleModifier.targets.FirstOrDefault() == null){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(MoveData.FAIL));
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

        if(moveData.moveName == "Thunder Wave" && target.pokemon.IsThisType(ReferenceLib.GetPokemonType("Ground"))){
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

        //if ALL move effects implement ICheckMoveEffectFail and they ALL fail, the move fails
        ICheckMoveEffectFail[] componentsCausingEffectToFail = turnAction.GetComponents<ICheckMoveEffectFail>();
        if(componentsCausingEffectToFail.Length > 0){
            int failCount = 0;
            foreach(ICheckMoveEffectFail effectThatMayFail in componentsCausingEffectToFail){
                if(effectThatMayFail.CheckMoveEffectFail(user, target, moveData)){
                    failCount++;
                }
            }
            MoveEffect[] allMoveEffects = turnAction.GetComponents<MoveEffect>();
            if(failCount == allMoveEffects.Length){
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

    public IEnumerator HandleSpikes(BattleTarget target){
        if(target.teamBattleModifier.spikesCount == 0 || SpikesEffect.IsTargetImmune(target)){
            yield break;
        }

        float spikesPercent = target.teamBattleModifier.spikesCount switch {
            1 => 0.125f,
            2 => 0.167f,
            _ => 0.25f,
        };

        yield return StartCoroutine(ChangeTargetHealth(target, -target.pokemon.GetPercentMaxHP(spikesPercent)));
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " is hurt by spikes!"));
    }

    //TEST END OF TURN EFFECTS
    public IEnumerator EndOfTurnEffects(List<BattleTarget> battleTargets){
        
        //multi-turn resolution
        yield return StartCoroutine(HandleMultiTurn(battleTargets));

        //weather
        yield return StartCoroutine(HandleWeather(battleTargets));
        
        //bind effects
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyBind>(battleTargets));

        //curse
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyCurse>(battleTargets));

        //timed effects
        yield return StartCoroutine(DoTimedEffects(battleTargets));

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
        yield return StartCoroutine(combatSystem.HandleTeamEffects());

        //protect, endure, trap
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyTrap>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyProtect>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyEndure>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyMagicCoat>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplySnatch>(battleTargets));
        yield return StartCoroutine(DoAppliedEffectOfType<ApplyHelpingHand>(battleTargets));

        ClearFlinch(battleTargets);

        //abilities that boost (speed boost)
    }

    private void ClearFlinch(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            b.individualBattleModifier.flinched = false;
        }
    }

    private IEnumerator DoTimedEffects(List<BattleTarget> battleTargets){
        for(int i = 0; i < battleTargets.Count; i++){
            List<TimedEffectInfo> shallowCopyForEnumeration = new List<TimedEffectInfo>(battleTargets[i].individualBattleModifier.timedEffects);
            foreach(TimedEffectInfo timedEffectInfo in shallowCopyForEnumeration){
                yield return StartCoroutine(timedEffectInfo.timedEffect.DoAppliedEffect(battleTargets[i], null));
            }
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
        yield return StartCoroutine(combatSystem.HandleFaint());
    }

    private IEnumerator HandleMultiTurn(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            if(b.individualBattleModifier.multiTurnInfo != null){
                yield return StartCoroutine(b.individualBattleModifier.multiTurnInfo.multiTurn.DoAppliedEffect(b, null));
            }
        }
    }

    private IEnumerator HandleWeather(List<BattleTarget> battleTargets){
        Weather weather = CombatSystem.Weather;
        if(CombatSystem.weatherTimer == 0){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(weather.textOnStop));
            CombatSystem.Weather = ReferenceLib.ActiveArea.weather;
            yield break;
        }

        if(CombatSystem.Weather != ReferenceLib.ActiveArea.weather){
            CombatSystem.weatherTimer--;
        }
        
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(weather.textOnContinue));

        if(weather.damageEveryTurn){
            foreach(BattleTarget b in battleTargets){
                
                bool takeDamage = true;
                foreach(PokemonType immuneType in weather.immuneTypes){
                    if(b.pokemon.IsThisType(immuneType)){
                        takeDamage = false;
                    }
                }

                if(takeDamage){
                    int damage = (int)(0.0625f * b.pokemon.stats[0]);
                    yield return StartCoroutine(ChangeTargetHealth(b, -damage));
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(b.GetName() + " " + weather.textOnDamage));
                }
            }

            yield return StartCoroutine(combatSystem.HandleFaint());
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

        yield return StartCoroutine(combatSystem.HandleFaint());
    }
}
