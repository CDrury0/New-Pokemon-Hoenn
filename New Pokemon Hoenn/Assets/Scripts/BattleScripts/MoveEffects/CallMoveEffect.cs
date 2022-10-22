using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallMoveEffect : MoveEffect
{
    public enum CallType {MirrorMove, SleepTalk, Assist}
    public CallType moveCallType;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException(); //check individualModifier.movesLastUsedAgainstThis
    }
}
