using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightBasedDamage : NormalDamage
{
    public bool basedOnUserWeight;
    public PiecewiseDamageData damagePairs;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        BattleTarget whoseWeight = basedOnUserWeight ? user : target;
        int power = damagePairs.GetPower(whoseWeight.pokemon.weight);
        yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
    }
}
