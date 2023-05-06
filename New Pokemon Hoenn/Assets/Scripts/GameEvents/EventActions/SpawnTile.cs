using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : EventAction
{
    protected override IEnumerator EventActionLogic(){
        if(Random.Range(0, 9) == 0){
            CombatLib.Instance.combatSystem.StartBattle(new Pokemon(ReferenceLib.Instance.activeArea.generationValues.grassSpawnInfo));
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => CombatSystem.BattleActive);
    }
}
