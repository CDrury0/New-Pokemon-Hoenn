using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseEffect : MoveEffect, ICheckMoveFail
{
    public List<MoveEffect> effectsIfGhost;
    public List<MoveEffect> effectsIfNotGhost;
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
