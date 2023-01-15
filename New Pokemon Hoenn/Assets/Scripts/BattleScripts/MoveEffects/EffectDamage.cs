using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect, ICheckMoveFail
{
    public bool makesContact;

    //account for sturdy, endure here
    protected IEnumerator ApplyDamage(MoveData moveData, BattleTarget user, BattleTarget target, int damage){
        bool endured = false;
        if(target.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyEndure) != null && damage == target.pokemon.CurrentHealth){
            endured = true;
            damage--;
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -damage));
        if(endured){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " endured the hit!"));
        }
        if(moveData.category == MoveData.Category.Physical){
            target.individualBattleModifier.physicalDamageTakenThisTurn += damage;
        }
        else{
            target.individualBattleModifier.specialDamageTakenThisTurn += damage;
        }
        yield return StartCoroutine(DoHitEffects(user, target, moveData));
    }

    //rough skin, effect spore, etc.
    private IEnumerator DoHitEffects(BattleTarget user, BattleTarget target, MoveData moveData){
        target.individualBattleModifier.lastOneToDealDamage = user;
        yield break;
    }

    public virtual string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(moveData.focusPunch && (user.individualBattleModifier.specialDamageTakenThisTurn > 0 || user.individualBattleModifier.physicalDamageTakenThisTurn > 0)){
            return user.GetName() + " lost its focus and couldn't move!";
        }
        if(moveData.category != MoveData.Category.Status && CombatLib.Instance.moveFunctions.GetTypeMatchup(moveData.GetEffectiveMoveType(), target) == 0){
            return "It doesn't affect " + target.GetName() + "...";
        }
        return null;
    }
}
