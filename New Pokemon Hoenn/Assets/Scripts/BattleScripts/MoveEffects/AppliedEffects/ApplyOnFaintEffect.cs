using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnFaintEffect {DestinyBond, Grudge}
public class ApplyOnFaintEffect : ApplyIndividualEffect, IApplyEffect
{
    public OnFaintEffect onFaintEffect;
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(target.pokemon.primaryStatus == PrimaryStatus.Fainted){
            MoveRecordList.MoveRecord recordOfAttack = CombatSystem.MoveRecordList.FindRecordLastAttacker(target.pokemon);
            if(recordOfAttack != null){
                if(onFaintEffect == OnFaintEffect.DestinyBond){
                    yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(CombatSystem.BattleTargets.Find(b => b.pokemon == recordOfAttack.user), -recordOfAttack.user.stats[0]));
                    yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " took its attacker down with it!"));
                }
                else{
                    int whichMove = recordOfAttack.user.moves.IndexOf(recordOfAttack.moveUsed);
                    recordOfAttack.user.movePP[whichMove] = 0;
                    yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(recordOfAttack.moveUsed.GetComponent<MoveData>().moveName + " lost all its PP due to the grudge!"));
                }
            }
        }
    }

    void Awake(){
        message = onFaintEffect == OnFaintEffect.DestinyBond ? "&userName is commanding its destiny!" : "&userName is bearing a grudge!";
    }
}
