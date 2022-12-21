using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipDamage : NormalDamage
{
    public bool frustration;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int power;
        power = !frustration ? (int)(user.pokemon.friendship / 2.5f) : (int)((255 - user.pokemon.friendship) / 2.5f);
        if(power == 0){
            power = 1;
        }
        yield return StartCoroutine(base.NormalDamageMethod(user, target, moveData, power));
    }
}
