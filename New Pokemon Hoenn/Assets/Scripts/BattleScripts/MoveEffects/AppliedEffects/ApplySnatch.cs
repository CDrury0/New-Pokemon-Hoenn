using System.Collections;

public class ApplySnatch : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        effectInfo.effect.RemoveEffect(target, effectInfo);
        yield break;
    }

    void Awake(){
        message = "&userName is ready to snatch a move!";
    }
}