using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveFunctions : MonoBehaviour
{
    public CombatScreen combatScreen;
    private const float WEAKNESS = 1.75f;
    private const float RESIST = 0.58f;

    public float GetTypeMatchup(StatLib.Type moveType, StatLib.Type defType1, StatLib.Type defType2){
        float ef1 = GetTypeEffectiveness(moveType, defType1);
        float ef2 = GetTypeEffectiveness(moveType, defType2);
        if(ef1 == WEAKNESS && ef2 == RESIST || ef1 == RESIST && ef2 == WEAKNESS){
            return 1f;
        }
        else{
            return ef1 * ef2;
        }
    }

    private float GetTypeEffectiveness(StatLib.Type offensiveType, StatLib.Type defensiveType){
        if(offensiveType == StatLib.Type.None){
            return 1;
        }
        ReferenceLib.TypeMatchupList matchupList = ReferenceLib.Instance.typeEffectivenessMatchups.First(type => type.attackingType == offensiveType);
        ReferenceLib.TypeMatchupValues values = matchupList.matchup.FirstOrDefault(type => type.defendingType == defensiveType);
        return values != null ? MatchTypeEffectivenessToValue(values.effectiveness) : 1;
    }

    private float MatchTypeEffectivenessToValue(StatLib.Matchup matchup){
        switch (matchup){
            case StatLib.Matchup.Weakness:
                return WEAKNESS;
            case StatLib.Matchup.Resistance:
                return RESIST;
            default:
                return 0;
        }
    }
}
