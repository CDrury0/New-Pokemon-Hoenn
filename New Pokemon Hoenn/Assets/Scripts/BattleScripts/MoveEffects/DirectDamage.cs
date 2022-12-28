using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectDamage : EffectDamage
{
    public bool bideRelease;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int damage = moveData.displayPower;
        if(bideRelease){
            damage = user.individualBattleModifier.bideDamage;
            user.individualBattleModifier.bideDamage = 0;
        }
        yield return StartCoroutine(ApplyDamage(moveData, user, target, damage));
    }
}
