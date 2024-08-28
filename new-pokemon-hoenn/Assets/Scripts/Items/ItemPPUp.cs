using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPPUp : ItemEffect, ICheckMoveItemUse
{
    private const int MAGIC_PP_NUMERATOR = 8;
    private const int MAGIC_PP_DENOMINATOR = 5;
    [SerializeField] private int numBoosts;

    public bool CanBeUsed(Pokemon p, int move) {
        return p.moveMaxPP[move] < p.moves[move].GetComponent<MoveData>().maxPP * MAGIC_PP_NUMERATOR / MAGIC_PP_DENOMINATOR;
    }

    public override bool CanEffectBeUsed(Pokemon p) {
        for(int i = 0; i < p.moves.Count; i++){
            if(CanBeUsed(p, i)){
                return true;
            }
        }
        return false;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput, int whichMove) {
        MoveData moveData = p.moves[whichMove].GetComponent<MoveData>();
        int amountAdded = (moveData.maxPP * numBoosts) / MAGIC_PP_DENOMINATOR;
        p.moveMaxPP[whichMove] += amountAdded;
        p.movePP[whichMove] += amountAdded;
        message = "Max PP for " + p.nickName + "'s " + moveData.moveName + " was increased";
        yield break;
    }
}
