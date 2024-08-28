using System.Collections;
using UnityEngine;

public class ApplyInfatuate : ApplyIndividualEffect, IApplyEffect
{
    public void RemoveIfInflictorSwitchedOut(BattleTarget target, AppliedEffectInfo effectInfo){
        if(!effectInfo.inflictor.individualBattleModifier.inflictingEffects.Contains(effectInfo)){
            effectInfo.effect.RemoveEffect(target, effectInfo);
        }
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.gender == Gender.None || target.pokemon.gender == user.pokemon.gender){
            return true;
        }
        //oblivious?
        return false;
    }

    void Awake(){
        message = "&targetName fell in love with &userName";
    }

    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        throw new System.NotImplementedException();
    }
}