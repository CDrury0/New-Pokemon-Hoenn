using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiveExperience : ItemEffect
{
    private HandleExperience expHandler;
    [SerializeField] private int expLevels;
    [SerializeField] private int flatExpAmount;

    public override bool CanEffectBeUsed(Pokemon p){
        return p.level < Pokemon.MAX_LEVEL;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, Func<string, IEnumerator> messageOutput, int whichMove){
        int expAmount = flatExpAmount;
        for(int i = 0; i < expLevels; i++){
            //the difference between next level's requirement and the previous level's requirement is the amount added for EACH level granted by the item
            int nextLevel = p.pokemonDefault.CalculateExperienceAtLevel(p.level + i + 1) - p.pokemonDefault.CalculateExperienceAtLevel(p.level + i);
            expAmount += nextLevel;
        }
        int startLevel = p.level;
        yield return StartCoroutine(expHandler.DoIndividualExperience(p, expAmount, (string message) => messageOutput(message)));
        if(startLevel != p.level){
            HandleEvolution handleEvolution = Instantiate(CombatLib.Instance.combatSystem.handleEvolutionObj).GetComponent<HandleEvolution>();
            yield return StartCoroutine(handleEvolution.EvolveMon(p));
            Destroy(handleEvolution.gameObject);
        }
    }

    void Awake() {
        expHandler = CombatLib.Instance.combatSystem.handleExperience;
    }
}
