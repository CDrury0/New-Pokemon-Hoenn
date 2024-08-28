using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostUserHealthEffect : MoveEffect, ICheckMoveFail, ICheckMoveEffectFail
{
    public bool curse;
    public bool canKillSelf;
    public float percentHealthCost;

    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData){
        return curse && !user.pokemon.IsThisType(ReferenceLib.GetPokemonType("Ghost"));
    }

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData){
        if(!canKillSelf && Mathf.RoundToInt(percentHealthCost * user.pokemon.stats[0]) >= user.pokemon.CurrentHealth){
            return MoveData.FAIL;
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData){
        int healthToRemove = (int)(percentHealthCost * user.pokemon.stats[0]);
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon, -healthToRemove));
        user.pokemon.CurrentHealth -= healthToRemove;
    }
}
