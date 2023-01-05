using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatScreen : MonoBehaviour
{
    [SerializeField] private RectTransform playerSingleTransform;
    [SerializeField] private RectTransform enemySingleTransform;
    [SerializeField] private RectTransform playerDoubleTransform;
    [SerializeField] private RectTransform enemyDoubleTransform;
    [SerializeField] private Button[] moveButtons;
    [SerializeField] private TextMeshProUGUI actionPromptText;
    public GameObject battleOptionsLayoutObject;
    public GameObject moveButtonLayoutObject;
    public GameObject moveBackButton;
    public GameObject targetBackButton;
    public ColorSO typeColors;
    public WriteText battleText;
    public GameObject player1Object;
    public GameObject enemy1Object;
    public GameObject player2Object;
    public GameObject enemy2Object;
    public BattleHUD player1hud;
    public BattleHUD player2hud;
    public BattleHUD enemy1hud;
    public BattleHUD enemy2hud;

    public void SetBattleSpriteFormat(bool isDoubleBattle){
        player1hud.gameObject.SetActive(false);
        player2hud.gameObject.SetActive(false);
        enemy1hud.gameObject.SetActive(false);
        enemy2hud.gameObject.SetActive(false);

        player1Object.SetActive(false);
        player2Object.SetActive(false);
        enemy1Object.SetActive(false);
        enemy2Object.SetActive(false);

        player1Object.GetComponent<RectTransform>().anchoredPosition = playerSingleTransform.anchoredPosition;
        enemy1Object.GetComponent<RectTransform>().anchoredPosition = enemySingleTransform.anchoredPosition;

        if(isDoubleBattle){
            player1Object.GetComponent<RectTransform>().anchoredPosition = playerDoubleTransform.anchoredPosition;
            enemy1Object.GetComponent<RectTransform>().anchoredPosition = enemyDoubleTransform.anchoredPosition;
        }
    }

    public void SetStartingGraphics(List<BattleTarget> battleTargets){
        foreach(BattleTarget b in battleTargets){
            b.battleHUD.SetBattleHUD(b.pokemon);
            b.monSpriteObject.GetComponent<Image>().sprite = b.teamBattleModifier.isPlayerTeam ? b.pokemon.backSprite : b.pokemon.frontSprite;
            b.battleHUD.gameObject.SetActive(true);
            b.monSpriteObject.SetActive(true);
        }
    }

    public void ShowMoveButtons(BattleTarget b){
        //check against ICheckMoveSelectable
        for(int i = 0; i < b.pokemon.moves.Capacity; i++){
            if(b.pokemon.moves[i] != null){
                moveButtons[i].GetComponent<Image>().color = typeColors.colors[(int)b.pokemon.moves[i].GetComponent<MoveData>().GetEffectiveMoveType()];
                TextMeshProUGUI text = moveButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = b.pokemon.moves[i].GetComponent<MoveData>().moveName;
                text.text += "  " + b.pokemon.movePP[i] + "/" + b.pokemon.moveMaxPP[i];
                moveButtons[i].interactable = true;
            }
            else{
                moveButtons[i].GetComponent<Image>().color = new Color(typeColors.colors[0].r, typeColors.colors[0].g, typeColors.colors[0].b, 127);
                moveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "---";
                moveButtons[i].interactable = false;
            }
        }
        moveButtonLayoutObject.SetActive(true);
        moveBackButton.SetActive(true);
    }

    public void HideMoveButtons(){
        moveButtonLayoutObject.SetActive(false);
        moveBackButton.SetActive(false);
    }

    public void HideTargetButtons(){
        player1Object.GetComponent<Button>().interactable = false;
        player2Object.GetComponent<Button>().interactable = false;
        enemy1Object.GetComponent<Button>().interactable = false;
        enemy2Object.GetComponent<Button>().interactable = false;
        targetBackButton.SetActive(false);
        battleText.gameObject.SetActive(false);
    }

    public void SetActionPromptText(){
        actionPromptText.text = "What should " + CombatSystem.ActiveTarget.GetName() + " do?";
    }
}
