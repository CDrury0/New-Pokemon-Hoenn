using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApplyEffect
{
    public abstract IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo);
}
