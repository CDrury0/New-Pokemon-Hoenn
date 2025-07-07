using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventWildBattle : EventAction
{
    // add fields for AI and music to allow special encounters via this component
    public Pokemon generatedMon;
    public EventAnimation encounterIntro;

    protected override IEnumerator EventActionLogic() {
        yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(generatedMon, encounterIntro));
        exit = !CombatSystem.PlayerVictory;
    }
}
