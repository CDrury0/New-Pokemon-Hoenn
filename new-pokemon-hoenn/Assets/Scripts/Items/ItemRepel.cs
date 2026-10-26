using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepel : ItemEffect
{
    [SerializeField] private int numSteps;
    
    public override bool CanEffectBeUsed(Pokemon p){
        return true;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        // Filters out SpawnInfo structs whose levelMax is not at least that of the party leader
        bool SpawnFilterByLevel(SpawnInfo spawnInfo){
            return spawnInfo.levelMax >= PlayerParty.GetLeader().level;
        }
        GenerationValues.spawnFilters.Add(SpawnFilterByLevel);

        // Forces the levelMin to be at least that of the party leader
        SpawnInfo SpawnModifyLevel(SpawnInfo spawnInfo){
            return spawnInfo.UpdateMinLevel(PlayerParty.GetLeader().level);
        }
        GenerationValues.spawnInfoModifiers.Add(SpawnModifyLevel);

        // Registers repel effect to be updated every step via PlayerInput
        var input = PlayerInput.Instance;
        int stepsTrigger = input.StepCount + numSteps;
        bool RepelStepHandler(){
            if (input.StepCount < stepsTrigger){
                return true;
            }
            input.UnregisterAfterStep(RepelStepHandler);
            GenerationValues.spawnFilters.Remove(SpawnFilterByLevel);
            GenerationValues.spawnInfoModifiers.Remove(SpawnModifyLevel);
            return true;
        }
        input.RegisterAfterStep(RepelStepHandler, 100);

        message = itemLogic.itemData.itemName + " will deter wild Pokémon for " + numSteps + " steps";
        yield break;
    }
}
