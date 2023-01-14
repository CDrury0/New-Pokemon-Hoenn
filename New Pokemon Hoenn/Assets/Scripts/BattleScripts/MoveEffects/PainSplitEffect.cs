using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainSplitEffect : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int targetHealth = (user.pokemon.CurrentHealth + target.pokemon.CurrentHealth) / 2;
        int healthChange = targetHealth - user.pokemon.CurrentHealth;
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(user, healthChange));
        healthChange = targetHealth - target.pokemon.CurrentHealth;
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, healthChange));
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage("The battlers shared their pain!"));
    }
}
