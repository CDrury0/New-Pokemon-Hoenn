using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeEV : ItemEffect
{
    [Tooltip("0 = HP")] [SerializeField] private int statToChange;
    [SerializeField] private int changeAmount;

    public override bool CanEffectBeUsed(Pokemon p){
        return changeAmount > 0 ? p.effortValues[statToChange] < Pokemon.MAX_EV : p.effortValues[statToChange] > 0;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput){
        if(changeAmount + p.effortValues[statToChange] > Pokemon.MAX_EV){
            p.effortValues[statToChange] = Pokemon.MAX_EV;
        }
        else if(changeAmount + p.effortValues[statToChange] < 0){
            p.effortValues[statToChange] = 0;
        }
        else{
            p.effortValues[statToChange] += changeAmount;
        }
        p.UpdateStats();
        message = p.nickName + "'s " + GetStatName(statToChange) + " was " + (changeAmount > 0 ? "increased" : "decreased");
        yield break;
    }

    private string GetStatName(int statNum){
        switch(statNum){
            case 0:
                return "HP";
            case 1:
                return "Attack";
            case 2:
                return "Defense";
            case 3:
                return "Special Attack";
            case 4:
                return "Special Defense";
            case 5:
                return "Speed";
            default:
                return "bruh";
        }
    }
}
