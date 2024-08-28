using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectDamage : EffectDamage
{
    public bool bideRelease;

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(bideRelease && user.individualBattleModifier.bideDamage == 0){
            return MoveData.FAIL;
        }
        return base.CheckMoveFail(user, target, moveData);
    }
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int damage = moveData.displayPower;
        if(bideRelease){
            damage = user.individualBattleModifier.bideDamage * 2;
            user.individualBattleModifier.bideDamage = 0;
        }
        yield return StartCoroutine(ApplyDamage(moveData, user, target, damage));
    }
}
