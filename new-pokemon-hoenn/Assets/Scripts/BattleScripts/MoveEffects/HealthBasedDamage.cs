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
    public bool waterSpoutTypeDamage;
    public PiecewiseDamageData piecewiseDamage;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        int power = waterSpoutTypeDamage ? WaterSpoutFormula(user, moveData.displayPower) : piecewiseDamage.GetPower((float)user.pokemon.CurrentHealth / (float)user.pokemon.stats[0]);
        yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(matchup));
    }

    private int WaterSpoutFormula(BattleTarget user, int displayPower){
        int power = (int)((float)(displayPower) * (float)(user.pokemon.CurrentHealth) / (float)(user.pokemon.stats[0]));
        return power > 1 ? power : 1;
    }
}
