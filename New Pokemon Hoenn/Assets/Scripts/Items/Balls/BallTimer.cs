using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTimer : ItemBall
{
    protected override float BallCatchRateMod(Pokemon user, Pokemon target) {
        return Mathf.Min((float)(CombatSystem.TurnCount + 10) / 10, 4f);
    }
}
