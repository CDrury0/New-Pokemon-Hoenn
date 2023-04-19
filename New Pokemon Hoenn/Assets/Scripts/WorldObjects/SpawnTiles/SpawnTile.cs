using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [SerializeField] private EnemyAI wildAI;

    void OnTriggerEnter2D (Collider2D collider) {
        if(collider.CompareTag("Player") && Random.Range(0, 9) == 0){
            CombatLib.Instance.combatSystem.StartBattle(new Pokemon(ReferenceLib.Instance.activeArea.generationValues.grassSpawnInfo), wildAI);
        }
    }
}
