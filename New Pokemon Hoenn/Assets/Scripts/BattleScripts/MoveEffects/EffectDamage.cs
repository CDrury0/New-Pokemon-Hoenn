using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    public int hitsMaxTimes;
    public bool makesContact;

    public virtual IEnumerator NormalDamageMethod(BattleTarget user, BattleTarget target, MoveData moveData, int power){
        //roll for crit
        //int damage = damageFormula(blah blah)
        //yield return StartCoroutine(moveFunctions.ApplyDamage(damage))
        yield break;
    }
}
