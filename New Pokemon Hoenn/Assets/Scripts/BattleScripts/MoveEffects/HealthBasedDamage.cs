using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class PiecewiseDamageData
{
    [SerializeField] private List<PiecewisePair> piecewisePairs;

    public int GetPower(float key){
        return piecewisePairs.Find(pair => key <= pair.key).powerValue;
    }

    [System.Serializable] public class PiecewisePair{
        public float key;
        public int powerValue;
    }
}
public class HealthBasedDamage : NormalDamage
{
    public PiecewiseDamageData piecewiseDamage;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int power = piecewiseDamage.GetPower((float)user.pokemon.CurrentHealth / (float)user.pokemon.stats[0]);
        yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
    }
}
