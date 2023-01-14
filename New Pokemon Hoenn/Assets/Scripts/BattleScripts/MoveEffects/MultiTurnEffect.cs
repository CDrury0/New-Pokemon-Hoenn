using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurnEffect : MoveEffect
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

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        user.individualBattleModifier.semiInvulnerable = givesSemiInvulnerable;
        if(forcedToUseMax > 0 && user.individualBattleModifier.ForcedToUseUntilCounter == 0){
            user.individualBattleModifier.ForcedToUseUntilCounter = alwaysUseMaxTurns ? forcedToUseMax : Random.Range(2, forcedToUseMax + 1);
        }
        if(mustRecharge){
            user.individualBattleModifier.recharging = true;
        }
        if(chargingTurn){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(ReplaceBattleMessage(user, target, moveData)));
        }
        if(chargingTurn && skipChargingTurn != Weather.None && CombatSystem.Weather == skipChargingTurn){
            GameObject nextMove = Instantiate(useNext);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.UseMove(user, nextMove, true, false));
        }
    }
}
