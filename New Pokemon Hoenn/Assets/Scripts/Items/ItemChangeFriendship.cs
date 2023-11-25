using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeFriendship : ItemEffect
{
    [SerializeField] private int changeAmount;

    public override bool CanEffectBeUsed(Pokemon p){
        return (changeAmount > 0 && p.Friendship < Pokemon.MAX_FRIENDSHIP)
        || (changeAmount < 0 && p.Friendship > 0);
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        p.Friendship += changeAmount;
        message = p.nickName + "'s friendship was " + (changeAmount > 0 ? "increased" : "decreased");
        yield break;
    }
}
