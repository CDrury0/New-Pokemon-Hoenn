using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPokemonType : ItemBall
{
    [SerializeField] private List<PokemonType> types;
    [SerializeField] private float boostedRate;

    protected override float BallCatchRateMod(Pokemon user, Pokemon target) {
        foreach(PokemonType type in types){
            if(target.IsThisType(type)){
                return boostedRate;
            }
        }
        return 1f;
    }
}
