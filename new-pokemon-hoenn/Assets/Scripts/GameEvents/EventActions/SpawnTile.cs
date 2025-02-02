using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : EventAction
{
    [SerializeField] private EventAnimation encounterIntro;
    [Tooltip("If left empty, value is inherited from parent GameAreaManager.areaData.generationValues")]
    [SerializeField] private GenerationValues generationValues;

    protected override IEnumerator EventActionLogic(){
        if(!SpawnerTriggered()){
            yield break;
        }
        Pokemon generatedMon = generationValues.GeneratePokemon();
        if(generatedMon == null){
            yield break;
        }
        yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(generatedMon, encounterIntro));
        exit = !CombatSystem.PlayerVictory;
    }

    // Include modifiers from stench, etc
    public static bool SpawnerTriggered(){
        return Random.Range(0, 9) == 0;
    }

    void Awake(){
        generationValues ??= GetComponentInParent<GameAreaManager>().areaData.defaultGenerationValues;
    }
}
