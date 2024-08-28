using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCurePrimaryStatus : ItemEffect
{
    public PrimaryStatus CureStatus { get { return _cureStatus; } }
    [SerializeField] private PrimaryStatus _cureStatus;

    public override bool CanEffectBeUsed(Pokemon p){
        return p.primaryStatus == _cureStatus || CanHealAny(p);
    }

    private bool CanHealAny(Pokemon p){
        //if the player has a primary status other than fainted and the item heals any condition, return true
        return p.primaryStatus != PrimaryStatus.None && p.primaryStatus != PrimaryStatus.Fainted && _cureStatus == PrimaryStatus.Any;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        p.primaryStatus = PrimaryStatus.None;
        message = p.nickName + " is no longer " + _cureStatus;
        yield break;
    }
}
