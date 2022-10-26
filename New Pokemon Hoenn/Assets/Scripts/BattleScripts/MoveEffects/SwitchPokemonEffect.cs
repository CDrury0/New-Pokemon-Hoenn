using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPokemonEffect : MoveEffect
{
    public bool passEffects;

    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException(); //choosing switch action prompts for the mon that will switch in. 
                                                    //if the individualModifier.switchingIn is null, prompt for the mon that will switch in.
    }
}
