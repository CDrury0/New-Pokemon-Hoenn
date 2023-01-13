using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesEffect : MoveEffect, ICheckMoveEffectFail
{
    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return CombatLib.Instance.combatSystem.BattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam).teamBattleModifier.spikesCount == TeamBattleModifier.SPIKES_MAX_STACKS;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        BattleTarget effectiveTarget = CombatLib.Instance.combatSystem.BattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam);
        effectiveTarget.teamBattleModifier.spikesCount++;
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(effectiveTarget.teamBattleModifier.teamPossessive + " side was covered in spikes!"));
    }
}
