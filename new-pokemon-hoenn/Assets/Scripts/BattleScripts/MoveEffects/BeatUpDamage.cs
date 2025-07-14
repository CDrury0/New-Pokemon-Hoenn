using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatUpDamage : NormalDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        var userTeam = CombatLib.CombatSystem.GetTeamParty(user).members;
        foreach(var member in userTeam){
            if(!member.IsFainted()){
                int power = (member.stats[1] / 10) + 5;
                yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
            }
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(matchup));
    }
}
