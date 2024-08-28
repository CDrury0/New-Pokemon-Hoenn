using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyHelpingHand : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        RemoveEffect(target, effectInfo);
        yield break;
    }

    void Awake(){
        message = "&userName is ready to help &targetName!";
    }
}
