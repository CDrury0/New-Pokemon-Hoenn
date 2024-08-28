using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRestorePP : ItemEffect, ICheckMoveItemUse
{
    [SerializeField] private int amountRestored;

    public bool CanBeUsed(Pokemon p, int move){
        return p.movePP[move] < p.moveMaxPP[move];
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
        message = "PP was restored";
        if(whichMove != -1){
            p.movePP[whichMove] = GetUpdatedValue(p.movePP[whichMove], p.moveMaxPP[whichMove], amountRestored);
            yield break;
        }
        //if the move index is -1, restore PP for all moves
        for(int i = 0; i < p.moves.Count; i++){
            p.movePP[i] = GetUpdatedValue(p.movePP[i], p.moveMaxPP[i], amountRestored);
        }
    }

    private int GetUpdatedValue (int startVal, int maxVal, int change) {
        return Mathf.Min(startVal + change, maxVal);
    }
}
