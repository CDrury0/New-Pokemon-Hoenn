using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRepeat : ItemBall
{
    [SerializeField] private float boostedRate;

    protected override float BallCatchRateMod(Pokemon user, Pokemon target) {
        return LoadDexInfo.globalDexProgress[target.pokemonDefault.IDNumber] == DexStatus.Caught ? boostedRate : 1f;
    }
}
