using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour, IInterruptPlayerMovement
{
    [SerializeField] private GenerationValues generationValues;
    [SerializeField] private EventTrigger trigger;
    [SerializeField] private EventWildBattle wildBattle;

    // This might need to be reworked to trigger on AfterStep rather than before
    public bool Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset) {
        SpawnerFunc();
        movementOffset = direction;
        return true;
    }

    protected void SpawnerFunc() {
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
}
