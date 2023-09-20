using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : EventAction
{
    [SerializeField] private EventAnimation encounterIntro;
    protected override IEnumerator EventActionLogic(){
        if(Random.Range(0, 9) == 0){
            Pokemon generatedMon = new Pokemon(ReferenceLib.Instance.activeArea.generationValues.grassSpawnInfo);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(generatedMon, encounterIntro));
            exit = !CombatSystem.PlayerVictory;
        }
    }
}
