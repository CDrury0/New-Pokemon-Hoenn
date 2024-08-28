using System.Collections;
using UnityEngine;

public class ApplyProtect : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        effectInfo.effect.RemoveEffect(target, effectInfo);
        yield break;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        user.individualBattleModifier.protectCounter++;
        yield return StartCoroutine(base.DoEffect(user, target, moveData));
    }

    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        for(int i = 0; i < user.individualBattleModifier.protectCounter; i++){
            if(Random.Range(0, 2) == 0){
                user.individualBattleModifier.protectCounter = 0;
                return true;
            }
        }
        return false;
    }

    void Awake(){
        message = "&userName protected itself!";
    }
}