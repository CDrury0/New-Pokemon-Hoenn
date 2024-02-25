using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatUpDamage : NormalDamage
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        Pokemon[] userTeam = CombatLib.Instance.combatSystem.GetTeamParty(user).party;
        for(int i = 0; i < userTeam.Length; i++){
            if(userTeam[i] != null && userTeam[i].primaryStatus != PrimaryStatus.Fainted){
                int power = (userTeam[i].stats[1] / 10) + 5;
                yield return StartCoroutine(NormalDamageMethod(user, target, moveData, power));
            }
        }
        yield return StartCoroutine(CombatLib.Instance.moveFunctions.WriteEffectivenessText(matchup));
    }
}
