using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffect : MoveEffect
{
    public PokemonType chargedType;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        target.individualBattleModifier.chargedType = chargedType;
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " charged up energy!"));
    }
}
