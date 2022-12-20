using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NormalDamage : EffectDamage
{
    public float recoilDamage;
    public float absorbHealth;
    public int damageDealt; //needed for absorbHealth and recoilDamage calculations
    public bool spitUp;
    public bool facade;
    public bool revenge;
    public bool furyCutter;
    public bool highCritRate;
    public bool bonusFromCurl;
    public bool bonusLikeRollout;
    public bool cannotKO;
    public bool bonusAgainstMinimize;
    public SemiInvulnerable bonusAgainstSemiInvulnerable;
    public PrimaryStatus bonusAgainstStatus;
    public bool curesBonusStatus;
    public bool payback;

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int power = moveData.displayPower;
        if(spitUp){
            power *= user.individualBattleModifier.stockpileCount;
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
        if(bonusAgainstMinimize && target.individualBattleModifier.appliedIndividualEffects.OfType<ApplyMinimize>().Any()){
            power *= 2;
        }
        if(bonusFromCurl && user.individualBattleModifier.appliedIndividualEffects.OfType<ApplyCurl>().Any()){
            power *= 2;
        }
        if(furyCutter){
            power *= 1 + user.individualBattleModifier.consecutiveMoveCounter <= 3 ? user.individualBattleModifier.consecutiveMoveCounter : 3;
        }
        if(bonusLikeRollout){
            power *= 1 + user.individualBattleModifier.consecutiveMoveCounter <= 5 ? user.individualBattleModifier.consecutiveMoveCounter : 5;
        }
        //payback

        yield return StartCoroutine(base.NormalDamageMethod(user, target, moveData, power, highCritRate, cannotKO));

        //recoil, absorb, cure target status

        damageDealt = 0;
    }
}
