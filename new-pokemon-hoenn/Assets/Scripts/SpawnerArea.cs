using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour, IInterruptPlayerMovement
{
    [Tooltip("If left empty, value is inherited from parent GameAreaManager.areaData.generationValues")]
    [SerializeField] private GenerationValues generationValues;
    [SerializeField] private EventTrigger trigger;
    [SerializeField] private EventWildBattle wildBattle;

    public void Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset) {
        SpawnerFunc(PlayerInput.StepCount);
        movementOffset = direction;
    }

    protected void SpawnerFunc(int stepCount) {
        if(!SpawnerTriggered())
            return;

        Pokemon generatedMon = generationValues.GeneratePokemon();
        if(generatedMon == null)
            return;

        wildBattle.generatedMon = generatedMon;
        trigger.DoTriggerActions();
    }

    // Include modifiers from stench, etc
    public static bool SpawnerTriggered(){
        return Random.Range(0, 9) == 0;
    }

    void Awake(){
        generationValues ??= GetComponentInParent<GameAreaManager>().areaData.defaultGenerationValues;
    }
}
