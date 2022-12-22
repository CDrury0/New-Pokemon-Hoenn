using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    public bool makesContact;

    //rough skin, effect spore, etc.
    protected IEnumerator DoHitEffects(){
        Debug.Log("hit effects");
        yield break;
    }

    protected IEnumerator ApplyDamage(MoveData moveData, BattleTarget user, BattleTarget target, int damage){
        yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon.CurrentHealth, target.pokemon.CurrentHealth - damage, target.pokemon.stats[0]));
        target.pokemon.CurrentHealth -= damage;
        if(moveData.category == MoveData.Category.Physical){
            target.individualBattleModifier.physicalDamageTakenThisTurn += damage;
        }
        else{
            target.individualBattleModifier.specialDamageTakenThisTurn += damage;
        }
    }
}
