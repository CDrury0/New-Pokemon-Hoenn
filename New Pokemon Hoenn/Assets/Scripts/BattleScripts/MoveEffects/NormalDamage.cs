using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NormalDamage : EffectDamage
{
    public int hitsMaxTimes;
    public float recoilDamage;
    public float absorbHealth;
    protected int damageDealt; //needed for absorbHealth and recoilDamage calculations
    public bool spitUp;
    public bool facade;
    public bool revenge;
    public bool furyCutter;
    public bool highCritRate;
    public bool bonusFromCurl;
    public bool bonusLikeRollout;
    public bool cannotKO;
    public bool cannotCrit;
    public bool bonusAgainstMinimize;
    public SemiInvulnerable bonusAgainstSemiInvulnerable;
    public PrimaryStatus bonusAgainstStatus;
    public bool curesBonusStatus;
    public bool payback;

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData){
        if(spitUp && user.individualBattleModifier.stockpileCount == 0){
            return MoveData.FAIL;
        }
        return base.CheckMoveFail(user, target, moveData);
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        int power = moveData.displayPower;
        if(spitUp){
            power *= user.individualBattleModifier.stockpileCount;
            user.individualBattleModifier.stockpileCount = 0;
        }
        if(facade && (int)user.pokemon.primaryStatus >= 1 && (int)user.pokemon.primaryStatus <= 3){
            power *= 2;
        }
        if(bonusAgainstSemiInvulnerable != SemiInvulnerable.None && bonusAgainstSemiInvulnerable == target.individualBattleModifier.semiInvulnerable){
            power *= 2;
        }
        if(bonusAgainstStatus != PrimaryStatus.None && bonusAgainstStatus == target.pokemon.primaryStatus){
            power *= 2;
        }
        if(revenge && (user.individualBattleModifier.specialDamageTakenThisTurn > 0 || user.individualBattleModifier.physicalDamageTakenThisTurn > 0)){
            power *= 2;
        }
        if(bonusAgainstMinimize && target.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyMinimize) != null){
            power *= 2;
        }
        if(bonusFromCurl && user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyCurl) != null){
            power *= 2;
        }
        if(furyCutter){
            power *= (1 + user.individualBattleModifier.consecutiveMoveCounter) <= 3 ? user.individualBattleModifier.consecutiveMoveCounter : 3;
        }
        if(bonusLikeRollout){
            power *= 1 + user.individualBattleModifier.consecutiveMoveCounter;
        }
        if(payback && CombatSystem.BattleTargets.Last() == user){
            power *= 2;
        }

        int timesToHit = 1;
        if(hitsMaxTimes != 0){
            timesToHit++;
            for(int i = timesToHit; i < hitsMaxTimes; i++){
                if(Random.Range(0, 10) < 5){
                    timesToHit++;
                }
            }
        }
        int timesHit = 0;
        while(timesHit < timesToHit){
            timesHit++;
            yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
            if(target.pokemon.CurrentHealth == 0){
                break;
            }
        }
        if(hitsMaxTimes != 0){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage("Hit " + timesHit + " time(s)!"));
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(matchup));

        if(recoilDamage != 0f){
            yield return StartCoroutine(DoRecoilDamage(user));
        }
        if(absorbHealth != 0f){
            yield return StartCoroutine(DoAbsorb(user, target));
        }
        if(curesBonusStatus && bonusAgainstStatus == target.pokemon.primaryStatus){
            yield return StartCoroutine(CureEnemyStatus(target));
        }

        damageDealt = 0;
    }

    private IEnumerator CureEnemyStatus(BattleTarget target){
        target.pokemon.primaryStatus = PrimaryStatus.None;
        target.battleHUD.SetBattleHUD(target.pokemon);
        yield return StartCoroutine(target.GetName() + " was cured!");
    }

    //add exception for liquid ooze
    private IEnumerator DoAbsorb(BattleTarget user, BattleTarget target){
        int actualAbsorbHealth = (int)(damageDealt * absorbHealth);
        if(actualAbsorbHealth < 1){
            actualAbsorbHealth = 1;
        }
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon, actualAbsorbHealth));
        user.pokemon.CurrentHealth += actualAbsorbHealth;
        yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage(target.GetName() + " had its energy drained!"));
    }

    private IEnumerator DoRecoilDamage(BattleTarget user){
        int actualRecoilDamage = (int)(damageDealt * recoilDamage);
        if(actualRecoilDamage < 1){
            actualRecoilDamage = 1;
        }
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon, -actualRecoilDamage));
        user.pokemon.CurrentHealth -= actualRecoilDamage;
        yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage(user.GetName() + " is damaged by recoil!"));
    }

    protected IEnumerator NormalDamageMethod(BattleTarget user, BattleTarget target, MoveData moveData, int power){
        bool crit = cannotCrit ? false : CombatLib.Instance.moveFunctions.RollCrit(user, highCritRate);
        int damage = NormalDamageFormula(power, user, target, crit);
        damageDealt += damage;
        yield return StartCoroutine(base.ApplyDamage(moveData, user, target, damage));
        if(crit){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage("A critical hit!"));
        }
    }

    protected int NormalDamageFormula(int power, BattleTarget user, BattleTarget target, bool crit){
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

        PokemonType localType = moveData.GetEffectiveMoveType(user.pokemon);

        modifier *=  localType.GetBattleEffectiveness(target);

        modifier *= user.pokemon.IsThisType(localType) ? 1.4f : 1f;

        modifier *= GetWeatherDamageModifier(localType, CombatSystem.Weather);

        if(user.individualBattleModifier.chargedType != null && user.individualBattleModifier.chargedType == localType){
            modifier *= 1.5f;
            user.individualBattleModifier.chargedType = null;
        }

        if(user.individualBattleModifier.targets.Count > 1){
            modifier *= 0.75f;
        }

        //add condition for guts
        if(user.pokemon.primaryStatus == PrimaryStatus.Burned && moveData.category == MoveData.Category.Physical){
            modifier *= 0.5f;
        }

        if(moveData.category == MoveData.Category.Physical && target.teamBattleModifier.teamEffects.FirstOrDefault(e => e.effect.durationEffect == TeamDurationEffect.Reflect) != null && moveData.gameObject.GetComponent<BreaksWalls>() == null){
            modifier *= 0.67f;
        }

        if(moveData.category == MoveData.Category.Special && target.teamBattleModifier.teamEffects.FirstOrDefault(e => e.effect.durationEffect == TeamDurationEffect.LightScreen) != null && moveData.gameObject.GetComponent<BreaksWalls>() == null){
            modifier *= 0.67f;
        }

        if(user.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyHelpingHand) != null){
            modifier *= 1.5f;
        }

        //if user selected pursuit and target is switching out
        if(moveData.pursuit && target.turnAction.CompareTag("Switch")){
            modifier *= 2f;
        }

        if(crit){ //if ability is sniper, 2.25
            modifier *= 1.5f;
        }

        workingDamage *= modifier;
        workingDamage *= Random.Range(0.9f, 1f);

        int damage = workingDamage > 1 ? (int)workingDamage : 1;
        if(damage > target.pokemon.CurrentHealth){
            damage = target.pokemon.CurrentHealth;
        }
        if(cannotKO && damage >= target.pokemon.CurrentHealth){
            damage = target.pokemon.CurrentHealth - 1;
        }
        return damage;
    }

    private float GetWeatherDamageModifier(PokemonType moveType, Weather weather){
        const float WEATHER_BONUS = 1.4f;
        const float WEATHER_PENALTY = 0.6f;
        if(weather.typeFromWeather == moveType){
            return WEATHER_BONUS;
        }
        if(weather.weakensType == moveType){
            return WEATHER_PENALTY;
        }
        return 1f;
    }
}
