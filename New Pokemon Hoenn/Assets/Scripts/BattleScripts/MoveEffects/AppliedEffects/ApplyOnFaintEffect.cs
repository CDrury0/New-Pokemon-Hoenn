using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnFaintEffect {DestinyBond, Grudge}
public class ApplyOnFaintEffect : ApplyIndividualEffect, IApplyEffect
{
    public OnFaintEffect onFaintEffect;

    //in this case, target is the one being made to faint, which is actually the user of the attack (see EffectDamage.DoHitEffects)
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        //since this effect is always self-applied, the inflictor is the one with this effect on them
        if(effectInfo.inflictor.pokemon.CurrentHealth == 0){
            if(onFaintEffect == OnFaintEffect.DestinyBond){
                yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -target.pokemon.stats[0]));
                yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(effectInfo.inflictor.GetName() + " took its attacker down with it!"));
            }
            else{
                int whichMove = target.pokemon.moves.IndexOf(target.turnAction);
                target.pokemon.movePP[whichMove] = 0;
                yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s " + target.turnAction.GetComponent<MoveData>().moveName + " lost all its PP due to the grudge!"));
            }
        }
    }

    void Awake(){
        message = onFaintEffect == OnFaintEffect.DestinyBond ? "&userName is commanding its destiny!" : "&userName is bearing a grudge!";
    }
}
