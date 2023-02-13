using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPokemonEffect : MoveEffect, ICheckMoveFail
{
    public bool passEffects;

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(passEffects && !CombatLib.Instance.combatSystem.GetTeamParty(user).HasAvailableFighter()){
            return MoveData.FAIL;
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(CombatLib.Instance.combatSystem.SwitchPokemon(user, passEffects));
    }
}
