using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect, ICheckMoveFail, IFlashImage
{
    public bool makesContact;
    protected float matchup;

    //account for sturdy, endure here
    protected IEnumerator ApplyDamage(MoveData moveData, BattleTarget user, BattleTarget target, int damage){
        matchup = moveData.GetEffectiveMoveType(user.pokemon).GetBattleEffectiveness(target);
        SFXLib sfx = SFXLib.Instance;
        AudioClip onHit = matchup < 1f ? sfx.notVeryEffective : matchup == 1f ? sfx.normalEffective : sfx.superEffective;
        AudioManager.Instance.PlaySoundEffect(onHit);

        IFlashImage magic = this as IFlashImage;
        yield return StartCoroutine(magic.DoImageFlash(target.monSpriteObject.GetComponent<Image>()));

        bool endured = false;
        if(target.individualBattleModifier.GetEffectInfoOfType<ApplyEndure>() != null && damage == target.pokemon.CurrentHealth){
            endured = true;
            damage--;
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -damage));
        if(endured){
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " endured the hit!"));
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

    public virtual string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData){
        if(moveData.focusPunch && (user.individualBattleModifier.specialDamageTakenThisTurn > 0 || user.individualBattleModifier.physicalDamageTakenThisTurn > 0)){
            return user.GetName() + " lost its focus and couldn't move!";
        }
        if(moveData.category != MoveData.Category.Status && moveData.GetEffectiveMoveType(user.pokemon).GetBattleEffectiveness(target) == 0){
            return "It doesn't affect " + target.GetName() + "...";
        }
        return null;
    }
}
