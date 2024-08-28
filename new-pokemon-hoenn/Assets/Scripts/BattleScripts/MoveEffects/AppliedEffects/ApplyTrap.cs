using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTrap : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(!effectInfo.inflictor.individualBattleModifier.inflictingEffects.Contains(effectInfo)){
            RemoveEffect(target, effectInfo);
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " is no longer trapped!"));
        }
    }

    void Awake(){
        message = "&targetName can no longer escape!";
    }
}
