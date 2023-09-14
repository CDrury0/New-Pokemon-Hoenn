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
    public OverlayTransition startBattleBars;
    public OverlayTransition endBattleFadeIn;
    public OverlayTransition endBattleFadeAway;

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

    public void ShowMoveButtons(Pokemon p, bool[] isSelectables){
        for(int i = 0; i < p.moves.Capacity; i++){
            PopulateMoveButton(p, i, isSelectables[i]);
        }
        moveButtonLayoutObject.SetActive(true);
        moveBackButton.SetActive(true);
    }

    private void PopulateMoveButton(Pokemon pokemon, int whichMove, bool isSelectable){
        Image buttonImage = moveButtons[whichMove].GetComponent<Image>();
        TextMeshProUGUI buttonText = moveButtons[whichMove].GetComponentInChildren<TextMeshProUGUI>();
        if(pokemon.moves[whichMove] == null){
            buttonText.text = "---";
            buttonImage.color = typeColors.colors[0];
        }
        else{
            MoveData moveData = pokemon.moves[whichMove].GetComponent<MoveData>();
            buttonText.text = moveData.moveName + " " + pokemon.movePP[whichMove] + "/" + pokemon.moveMaxPP[whichMove];
            buttonImage.color = typeColors.colors[(int)moveData.GetEffectiveMoveType(pokemon)];
        }
        moveButtons[whichMove].interactable = isSelectable;
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
