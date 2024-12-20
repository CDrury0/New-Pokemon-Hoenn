using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference Objects/CombatEventHelper")]
public class CombatEventHelper : ScriptableObject
{
    public void Proceed(){
        CombatSystem.Proceed = true;
    }

    public void SetPlayerBattleOptions(bool state){
        CombatLib.Instance.combatScreen.battleOptionsLayoutObject.SetActive(state);
    }
}
