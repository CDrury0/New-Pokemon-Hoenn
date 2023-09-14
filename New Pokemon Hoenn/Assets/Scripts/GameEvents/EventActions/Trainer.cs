using System.Collections;
using UnityEngine;

public class Trainer : EventAction
{
    public bool isDoubleBattle;
    public string trainerTitle;
    public string trainerName;
    public string victoryMessage;
    public int rewardDollars;
    public EnemyAI trainerAI;
    public Sprite trainerSprite;
    public AudioPlayer battleMusic;
    public AudioPlayer victoryMusic;
    public OverlayTransition introAnimation;
    public SerializablePokemon[] trainerPartyTemplate = new SerializablePokemon[6];

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
                        trainerPartyTemplate[i].moveMaxPP[j] = move.GetComponent<MoveData>().maxPP;
                    }
                }
            }
        }
    }

    protected override IEnumerator EventActionLogic() {
        yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(this));
        exit = !CombatSystem.PlayerVictory;
    } 
}
