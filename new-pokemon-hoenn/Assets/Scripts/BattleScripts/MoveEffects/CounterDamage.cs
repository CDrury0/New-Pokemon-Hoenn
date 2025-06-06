using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterDamage : EffectDamage, ICheckMoveFail
{
    public bool mirrorCoat;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int damage = 2 * (mirrorCoat ? user.individualBattleModifier.specialDamageTakenThisTurn : user.individualBattleModifier.physicalDamageTakenThisTurn);
        yield return StartCoroutine(ApplyDamage(moveData, user, target, damage));
    }

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int relevantDamageTaken = mirrorCoat ? user.individualBattleModifier.specialDamageTakenThisTurn : user.individualBattleModifier.physicalDamageTakenThisTurn;
        if(relevantDamageTaken == 0){
            return MoveData.FAIL;
        }
        return base.CheckMoveFail(user, target, moveData);
    }
}
