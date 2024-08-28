using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon Type")]
public class PokemonType : ScriptableObject
{
    public Color typeColor;
    [SerializeField] private List<PokemonType> superEffectiveAgainst;
    [SerializeField] private List<PokemonType> resistedBy;
    [SerializeField] private List<PokemonType> immuneToThis;
    private const float TYPE_WEAKNESS = 1.75f;
    private const float TYPE_RESIST = 0.58f;

    public static string GetMatchupMessage(float effectiveness){
        if(effectiveness > 1f){
            return "It's super effective!";
        }
        if(effectiveness < 1f){
            return "It's not very effective...";
        }
        return string.Empty;
    }

    public float GetBattleEffectiveness(BattleTarget defender){
        if(defender.individualBattleModifier.GetEffectInfoOfType<ApplyIdentify>() != null){
            return 1f;
        }
        return GetSimpleEffectiveness(defender.pokemon);
    }

    public float GetSimpleEffectiveness(Pokemon defender){
        float ef1 = GetSingleMatchup(defender.type1);
        float ef2 = GetSingleMatchup(defender.type2);
        if(ef1 == TYPE_WEAKNESS && ef2 == TYPE_RESIST || ef1 == TYPE_RESIST && ef2 == TYPE_WEAKNESS){
            return 1f;
        }
        return ef1 * ef2;
    }

    private float GetSingleMatchup(PokemonType defensive){
        if(defensive == null){
            return 1f;
        }
        if(superEffectiveAgainst.Contains(defensive)){
            return TYPE_WEAKNESS;
        }
        if(resistedBy.Contains(defensive)){
            return TYPE_RESIST;
        }
        if(immuneToThis.Contains(defensive)){
            return 0f;
        }
        return 1f;
    }
}
