using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDrowsy : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            if(target.pokemon.primaryStatus == PrimaryStatus.None && !ApplyPrimaryStatusEffect.ImmuneToStatus(PrimaryStatus.Asleep, target, false)){
                target.pokemon.primaryStatus = PrimaryStatus.Asleep;
                target.pokemon.sleepCounter = Random.Range(1, 5);
                yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " fell Asleep!"));
                target.battleHUD.SetBattleHUD(target.pokemon);
            }
            else{
                yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(target.GetName() + " resisted Sleep!"));
            }
            RemoveEffect(target, effectInfo);
        }
        else{
            effectInfo.timer--;
        }
    }

    void Awake(){
        message = "&targetName became drowsy!";
    }
}