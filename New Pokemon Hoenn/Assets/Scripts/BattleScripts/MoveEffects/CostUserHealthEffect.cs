using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostUserHealthEffect : MoveEffect, ICheckMoveFail
{
    public bool canKillSelf;
    public float percentHealthCost;

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(!canKillSelf && Mathf.RoundToInt(percentHealthCost * user.pokemon.stats[0]) >= user.pokemon.CurrentHealth){
            return "But it failed!";
        }
        return null;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int healthToRemove = Mathf.RoundToInt(percentHealthCost * user.pokemon.stats[0]);
        yield return StartCoroutine(user.battleHUD.healthBar.SetHealthBar(user.pokemon.CurrentHealth, user.pokemon.CurrentHealth - healthToRemove, user.pokemon.stats[0]));
        user.pokemon.CurrentHealth -= healthToRemove;
    }
}
