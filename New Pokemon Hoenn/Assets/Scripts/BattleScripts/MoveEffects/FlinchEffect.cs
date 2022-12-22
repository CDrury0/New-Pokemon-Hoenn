using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlinchEffect : MoveEffect
{
    public float chance;
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        bool immuneToFlinch = ImmuneToFlinch(target);
        if(chance == 1f && (immuneToFlinch || target.individualBattleModifier.flinched)){
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage(target.GetName() + " didn't flinch!"));
        }
        else if(Random.Range(0f, 1f) < chance && !immuneToFlinch){
            target.individualBattleModifier.flinched = true;
            yield return StartCoroutine(CombatLib.Instance.combatSystem.combatScreen.battleText.WriteMessage(target.GetName() + " flinched!"));
        }
    }

    private bool ImmuneToFlinch(BattleTarget target){
        //if opponent has inner focus, etc.
        return false;
    }
}
