using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    [Tooltip("If left empty, value is inherited from parent GameAreaManager.areaData.generationValues")]
    [SerializeField] private GenerationValues generationValues;
    [SerializeField] private EventAnimation encounterIntro;
    [SerializeField] private EventBattle eventBattle;

    void OnTriggerEnter2D(Collider2D collider) {
        if(!collider.gameObject.CompareTag("MovePoint"))
            return;

        PlayerInput.StepEvent += SpawnerFunc;
        Debug.Log("added spawner func to step event");
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(!collider.gameObject.CompareTag("MovePoint"))
            return;

        PlayerInput.StepEvent -= SpawnerFunc;
        Debug.Log("removed spawner func from step event");
    }

    protected void SpawnerFunc(int stepCount) {
        if(!SpawnerTriggered())
            return;

        Pokemon generatedMon = generationValues.GeneratePokemon();
        if(generatedMon == null)
            return;

        eventBattle.StartBattle(generatedMon, encounterIntro);
    }

    // Include modifiers from stench, etc
    public static bool SpawnerTriggered(){
        return Random.Range(0, 9) == 0;
    }

    void Awake(){
        generationValues ??= GetComponentInParent<GameAreaManager>().areaData.defaultGenerationValues;
    }
}
