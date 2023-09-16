using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApplyEffect
{
    /// <param name="target">target here refers to the mon currently affected</param>
    public abstract IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo);
}
