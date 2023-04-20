using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLib : MonoBehaviour
{
    public static CombatLib Instance {get; private set;}
    public MoveFunctions moveFunctions;
    public CombatSystem combatSystem;
    public CombatScreen combatScreen;

    void Awake(){
        if(Instance != null){
            Debug.Log("CombatLib exists");
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Replace with an equivalent that prints to a different object if a battle is not happening (e.g. move learned via TM)
    /// </summary>
    public IEnumerator WriteGlobalMessage(string message){
        if(CombatSystem.BattleActive){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(message));
        }
    }
}
