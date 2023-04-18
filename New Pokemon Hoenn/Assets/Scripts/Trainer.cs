using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trainer : MonoBehaviour
{
    public bool isDoubleBattle;
    public EnemyAI enemyAI;
    public SerializablePokemon[] trainerPartyTemplate = new SerializablePokemon[6];
    public UnityEvent actionsOnInteract;

    void OnTriggerEnter2D (Collider2D collider) {
        CombatLib.Instance.combatSystem.StartTrainerBattle(trainerPartyTemplate, isDoubleBattle, enemyAI);
    }

    void Awake() {
        for (int i = 0; i < trainerPartyTemplate.Length; i++){
            if(trainerPartyTemplate[i].pokemonDefault == null){
                trainerPartyTemplate[i] = null;
                continue;
            }
            for (int j = 0; j < trainerPartyTemplate[i].moves.Count; j++){
                if(trainerPartyTemplate[i].moveMaxPP[j] == 0){
                    GameObject move = trainerPartyTemplate[i].moves[j];
                    if (move != null){
                        MoveData moveData = move.GetComponent<MoveData>();
                        if (moveData != null){
                            trainerPartyTemplate[i].moveMaxPP[j] = moveData.maxPP;
                        }
                    }
                }
            }
        }
    }
}
