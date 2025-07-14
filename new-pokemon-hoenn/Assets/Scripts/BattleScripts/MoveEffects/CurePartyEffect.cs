using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurePartyEffect : MoveEffect
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData) {
        var userParty = CombatLib.CombatSystem.GetTeamParty(user).members;
        foreach(var member in userParty){
            //check for soundproof on heal bell
            if(!member.IsFainted())
                member.primaryStatus = PrimaryStatus.None;
        }
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(user.GetName() + "'s party was cured of status conditions!"));
        user.battleHUD.SetBattleHUD(user.pokemon);
    }
}
