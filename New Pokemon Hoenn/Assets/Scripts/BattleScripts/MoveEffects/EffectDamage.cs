using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    public int hitsMaxTimes;
    public bool makesContact;

    public virtual IEnumerator NormalDamageMethod(BattleTarget user, BattleTarget target, MoveData moveData, int power, bool highCritRate, bool cannotKO){
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
            yield return StartCoroutine(PrivateNormalDamageMethod(user, target, moveData, power, highCritRate, cannotKO));
            //rough skin?
        }
        if(hitsMaxTimes != 0){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage("Hit " + timesHit + " time(s)!"));
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(moveData, target));
    }

    protected virtual IEnumerator PrivateNormalDamageMethod(BattleTarget user, BattleTarget target, MoveData moveData, int power, bool highCritRate, bool cannotKO){
        bool crit = CombatLib.Instance.moveFunctions.RollCrit(user, highCritRate);
        int damage = CombatLib.Instance.moveFunctions.NormalDamageFormula(power, user, target, crit, cannotKO);
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ApplyDamage(moveData, user, target, damage));
        if(crit){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage("A critical hit!"));
        }
    }
}
