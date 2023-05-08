using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : EventAction
{
    protected override IEnumerator EventActionLogic(){
        if(Random.Range(0, 9) == 0){
            Pokemon generatedMon = new Pokemon(ReferenceLib.Instance.activeArea.generationValues.grassSpawnInfo);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(generatedMon));
            exit = !CombatSystem.PlayerVictory;
        }
    }
}
