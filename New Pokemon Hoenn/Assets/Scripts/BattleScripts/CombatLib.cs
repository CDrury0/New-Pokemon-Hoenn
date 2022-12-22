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

    public IEnumerator WriteBattleMessage(string message){
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(message));
    }
}
