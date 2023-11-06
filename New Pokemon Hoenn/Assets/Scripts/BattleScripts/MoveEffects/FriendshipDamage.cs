using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipDamage : NormalDamage
{
    public bool frustration;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int power;
        power = !frustration ? (int)(user.pokemon.Friendship / 2.5f) : (int)((255 - user.pokemon.Friendship) / 2.5f);
        if(power == 0){
            power = 1;
        }
        yield return StartCoroutine(base.NormalDamageMethod(user, target, moveData, power));
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(target, moveData.GetEffectiveMoveType(user.pokemon)));
    }
}
