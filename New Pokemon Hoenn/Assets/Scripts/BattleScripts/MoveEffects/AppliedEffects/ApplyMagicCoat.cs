using System.Collections;

public class ApplyMagicCoat : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        effectInfo.effect.RemoveEffect(target, effectInfo);
        yield break;
    }

    void Awake(){
        message = "&userName shrouded itself with &moveName";
    }
}