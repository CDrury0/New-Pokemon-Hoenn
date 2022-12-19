using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    public int hitsMaxTimes;
    public bool makesContact;

    public virtual IEnumerator NormalDamageMethod(BattleTarget user, BattleTarget target, NormalDamage damageComponent, MoveData moveData, int power, bool highCritRate){
        int damage = CombatLib.Instance.moveFunctions.NormalDamageFormula(power, damageComponent, user, target, CombatLib.Instance.moveFunctions.RollCrit(user, highCritRate));
        //yield return StartCoroutine(moveFunctions.ApplyDamage(damage))
        yield break;
    }
}
