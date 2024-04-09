using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesEffect : MoveEffect, ICheckMoveEffectFail
{
    public bool CheckMoveEffectFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return CombatSystem.BattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam).teamBattleModifier.spikesCount == TeamBattleModifier.SPIKES_MAX_STACKS;
    }

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        BattleTarget effectiveTarget = CombatSystem.BattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam);
        effectiveTarget.teamBattleModifier.spikesCount++;
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(effectiveTarget.teamBattleModifier.teamPossessive + " side was covered in spikes!"));
    }

    /// <returns>Whether or not the provided target is immune to damage from switching into spikes</returns>
    public static bool IsTargetImmune(BattleTarget target){
        if(!target.pokemon.IsThisType("Flying")){
            return false;
        }
        if(target.individualBattleModifier.GetEffectInfoOfType<ApplyIngrain>() != null){
            return false;
        }
        return true;
    }
}
