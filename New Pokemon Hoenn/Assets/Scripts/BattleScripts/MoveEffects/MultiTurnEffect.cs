using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurnEffect : MoveEffect, IApplyEffect
{
    public Weather skipChargingTurn;
    public GameObject useNext;
    public GameObject baseMove;
    public SemiInvulnerable givesSemiInvulnerable;
    public int forcedToUseMax;
    public bool alwaysUseMaxTurns;
    public bool confuseOnEnd;
    public bool bideCharge;
    public bool mustRecharge;
    public bool chargingTurn;

    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(bideCharge){
            target.individualBattleModifier.bideDamage += target.individualBattleModifier.physicalDamageTakenThisTurn;
            target.individualBattleModifier.bideDamage += target.individualBattleModifier.specialDamageTakenThisTurn;
        }
        int forcedToUse = target.individualBattleModifier.multiTurnInfo.forcedToUseUntilCounter;
        if(forcedToUse != 0){
            if(forcedToUse == target.individualBattleModifier.consecutiveMoveCounter){
                if(confuseOnEnd){
                    yield return StartCoroutine(CombatLib.Instance.moveFunctions.confuseAfterForcedToUse.DoEffect(target, target, CombatLib.Instance.moveFunctions.confuseAfterForcedToUseData));
                }
                target.individualBattleModifier.multiTurnInfo = null;
            }
        }
        else if(target.individualBattleModifier.multiTurnInfo.recharging){
            if(target.individualBattleModifier.consecutiveMoveCounter == 1){
                target.individualBattleModifier.multiTurnInfo = null;
            }
        }
        else if(useNext == null){
            target.individualBattleModifier.multiTurnInfo = null;
        }
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(chargingTurn){
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(ReplaceBattleMessage(user, target, moveData)));
        }
        if(chargingTurn && skipChargingTurn != null && CombatSystem.Weather == skipChargingTurn){
            GameObject nextMove = Instantiate(useNext);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, nextMove, true, false));
            yield break;
        }

        user.individualBattleModifier.semiInvulnerable = givesSemiInvulnerable;

        if(forcedToUseMax != 0){
            if(user.individualBattleModifier.multiTurnInfo == null){
                user.individualBattleModifier.consecutiveMoveCounter = 0;
                user.individualBattleModifier.multiTurnInfo = new MultiTurnInfo(this, alwaysUseMaxTurns ? forcedToUseMax : Random.Range(2, forcedToUseMax + 1), mustRecharge);
            }
        }
        else{
            user.individualBattleModifier.multiTurnInfo = new MultiTurnInfo(this, 0, false);
            user.individualBattleModifier.multiTurnInfo.useNext = useNext;
        }
    }
}
