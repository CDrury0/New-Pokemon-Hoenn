using System.Collections;
using UnityEngine;

public class Trainer : InteractEvent
{
    public bool isDoubleBattle;
    public EnemyAI enemyAI;
    public SerializablePokemon[] trainerPartyTemplate = new SerializablePokemon[6];

    void Awake() {
        for (int i = 0; i < trainerPartyTemplate.Length; i++){
            for (int j = 0; j < trainerPartyTemplate[i].moves.Count; j++){
                if(trainerPartyTemplate[i].moveMaxPP[j] == 0){
                    GameObject move = trainerPartyTemplate[i].moves[j];
                    if (move != null){
                        trainerPartyTemplate[i].moveMaxPP[j] = move.GetComponent<MoveData>().maxPP;
                    }
                }
            }
        }
    }

    public override IEnumerator DoInteractEvent() {
        CombatLib.Instance.combatSystem.StartBattle(this);
        yield break;
    }
}
