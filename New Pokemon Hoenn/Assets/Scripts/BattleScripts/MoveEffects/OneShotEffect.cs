using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffect : EffectDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(ApplyDamage(moveData, user, target, target.pokemon.stats[0]));
        if(target.pokemon.CurrentHealth == 0){
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage("It's a one-hit KO!"));
        }
    }

    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.IsThisType(StatLib.Type.Ice)){
            return MoveData.FAIL;
        }
        if(!OneShotHits(user, target)){
            return MoveData.FAIL;
        }
        return base.CheckMoveFail(user, target, moveData);
    }

    private bool OneShotHits(BattleTarget user, BattleTarget target){
        if(user.pokemon.level < target.pokemon.level){
            return false;
        }
        return Random.Range(0f, 1f) <= (float)(user.pokemon.level - target.pokemon.level + 30) / 100f;
    }
}
