using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStandard : ItemBall
{
    [SerializeField] private float catchRateMod = 1f;

    protected override float BallCatchRateMod(Pokemon user, Pokemon target) {
        return catchRateMod;
    }
}
