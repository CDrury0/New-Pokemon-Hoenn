using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventBattle : EventAction
{
    private Pokemon generatedMon;
    private EventAnimation encounterIntro;

    public void StartBattle(Pokemon generatedMon, EventAnimation encounterIntro) {
        this.generatedMon = generatedMon;
        this.encounterIntro = encounterIntro;
        StartCoroutine(DoEventAction(ScriptableObject.CreateInstance<EventState>()));
    }

    protected override IEnumerator EventActionLogic(EventState state) {
        yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(generatedMon, encounterIntro));
        exit = !CombatSystem.PlayerVictory;
    }
}
