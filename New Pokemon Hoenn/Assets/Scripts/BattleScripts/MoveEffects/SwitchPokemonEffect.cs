using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPokemonEffect : MoveEffect
{
    public bool passEffects;

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(CombatLib.Instance.combatSystem.SwitchPokemon(user));
    }
}
