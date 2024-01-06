using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trainer : EventAction
{
    public bool isDoubleBattle;
    [SerializeField] private string trainerTitle;
    [SerializeField] private string trainerName;
    public string victoryMessage;
    public int rewardDollars;
    public EnemyAI trainerAI;
    public Sprite trainerSprite;
    public AudioPlayer battleMusic;
    public AudioPlayer victoryMusic;
    public EventAnimation introAnimation;
    public SerializablePokemon[] trainerPartyTemplate = new SerializablePokemon[6];
    [HideInInspector] public List<ItemData> battleInventory;
    [SerializeField] private List<ItemData> inventoryTemplate;

    public string GetName(){
        return trainerTitle + " " + trainerName;
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
                        trainerPartyTemplate[i].moveMaxPP[j] = move.GetComponent<MoveData>().maxPP;
                    }
                }
            }
        }
    }

    protected override IEnumerator EventActionLogic() {
        battleInventory = new List<ItemData>(inventoryTemplate);
        yield return StartCoroutine(CombatLib.Instance.combatSystem.StartBattle(this));
        exit = !CombatSystem.PlayerVictory;
    }

    void OnValidate() {
        foreach(SerializablePokemon sp in trainerPartyTemplate){
            int total = sp.effortValues.Sum();
            if(total > Pokemon.MAX_EV_TOTAL){
                int which = Array.IndexOf(sp.effortValues, sp.effortValues.First(i => i > 0));
                sp.effortValues[which] -= total - Pokemon.MAX_EV_TOTAL;
                total = Pokemon.MAX_EV_TOTAL;
            }
            sp.evTotal = total;
        }
    }
}
