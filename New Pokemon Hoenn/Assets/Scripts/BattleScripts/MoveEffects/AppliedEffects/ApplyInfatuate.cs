using System.Collections;

public class ApplyInfatuate : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(!effectInfo.inflictor.individualBattleModifier.inflictingEffects.Contains(effectInfo)){
            effectInfo.effect.RemoveEffect(target, effectInfo);
        }
        yield break;
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
}