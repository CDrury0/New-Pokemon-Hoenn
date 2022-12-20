using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    public int hitsMaxTimes;
    public bool makesContact;

    public virtual IEnumerator NormalDamageMethod(BattleTarget user, BattleTarget target, NormalDamage damageComponent, MoveData moveData, int power, bool highCritRate){
        bool crit = CombatLib.Instance.moveFunctions.RollCrit(user, highCritRate);
        int damage = CombatLib.Instance.moveFunctions.NormalDamageFormula(power, damageComponent, user, target, crit);
        if(crit){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage("A critical hit!"));
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ApplyDamage(moveData, user, target, damage));
    }
}
