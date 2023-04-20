using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurePartyEffect : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        Pokemon[] userParty = CombatLib.Instance.combatSystem.GetTeamParty(user).party;
        for(int i = 0; i < userParty.Length; i++){
            //check for soundproof on heal bell
            if(userParty[i].primaryStatus != PrimaryStatus.Fainted){
                userParty[i].primaryStatus = PrimaryStatus.None;
            }
        }
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(user.GetName() + "'s party was cured of status conditions!"));
    }
}
