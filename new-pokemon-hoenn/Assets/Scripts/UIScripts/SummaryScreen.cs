using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SummaryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] statTexts;
    [SerializeField] private SummaryMovePlate[] movePlates;
    [SerializeField] private TextMeshProUGUI moveDescription;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private TextMeshProUGUI abilityName;

    public void ShowSummaryScreen(int whichPokemon) {
        Pokemon p = PlayerParty.Party.members.ElementAtOrDefault(whichPokemon);
        for (int i = 0; i < p.stats.Length; i++){
            statTexts[i].text = p.stats[i].ToString();
            float[] natureMultipliers = p.nature.GetNatureModifiers();
            if(natureMultipliers[i] > 1f){
                statTexts[i].color = new Color(0, 0, 255);
            }
            else if(natureMultipliers[i] < 1f){
                statTexts[i].color = new Color(255, 0, 0);
            }
            else{
                statTexts[i].color = new Color(255, 255, 255);
            }
        }

        for (int i = 0; i < movePlates.Length; i++){
            if(p.moves[i] != null){
                movePlates[i].SetMoveInfo(p.movePP[i], p.moveMaxPP[i], p.moves[i].GetComponent<MoveData>(), p);
                movePlates[i].gameObject.SetActive(true);
            }
            else{
                movePlates[i].gameObject.SetActive(false);
            }
        }
        
        abilityName.text = p.ability.ToString();
        abilityDescription.text = StatLib.GetAbilityDescription(p.ability);
        moveDescription.text = "Select a move to learn more about it.";
        gameObject.SetActive(true);
    }
}
