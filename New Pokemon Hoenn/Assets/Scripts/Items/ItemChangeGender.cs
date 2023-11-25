using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeGender : ItemEffect
{
    public override bool CanEffectBeUsed(Pokemon p) {
        return p.gender != Gender.None && CanBeEitherGender(p.pokemonDefault);
    }

    private bool CanBeEitherGender(PokemonDefault p) {
        return p.genderRatio < 1f && p.genderRatio > 0f;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput, int whichMove) {
        p.gender = p.gender == Gender.Male ? Gender.Female : Gender.Male;
        message = p.nickName + " became " + p.gender.ToString();
        yield break;
    }
}
