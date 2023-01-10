using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyIngrain : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        int ingrainHeal = (int)(target.pokemon.stats[0] * 0.1f);
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, ingrainHeal));
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " healed with its roots!"));
    }
}