using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect, ICheckMoveFail
{
    public bool makesContact;

    protected IEnumerator ApplyDamage(MoveData moveData, BattleTarget user, BattleTarget target, int damage){
        yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon.CurrentHealth, target.pokemon.CurrentHealth - damage, target.pokemon.stats[0]));
        target.pokemon.CurrentHealth -= damage;
        if(moveData.category == MoveData.Category.Physical){
            target.individualBattleModifier.physicalDamageTakenThisTurn += damage;
        }
        else{
            target.individualBattleModifier.specialDamageTakenThisTurn += damage;
        }
        yield return StartCoroutine(DoHitEffects());
    }

    //rough skin, effect spore, etc.
    private IEnumerator DoHitEffects(){
        Debug.Log("hit effects");
        yield break;
    }

    public virtual string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(moveData.category != MoveData.Category.Status && CombatLib.Instance.moveFunctions.GetTypeMatchup(moveData.GetEffectiveMoveType(), target.pokemon.type1, target.pokemon.type2) == 0){
            return "It doesn't affect " + target.GetName() + "...";
        }
        return null;
    }
}
