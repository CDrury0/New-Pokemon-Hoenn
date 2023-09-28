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
    public Image enemyTrainerImage;
    public BattleHUD player1hud;
    public BattleHUD player2hud;
    public BattleHUD enemy1hud;
    public BattleHUD enemy2hud;
    public EventAnimation barsOpening;
    [SerializeField] private SingleAnimOverride playerPlatform;
    [SerializeField] private SingleAnimOverride enemyPlatform;

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

    public void SetStartingGraphics(BattleTarget wildMon){
        battleText.gameObject.SetActive(false);
        enemyTrainerImage.gameObject.SetActive(CombatSystem.EnemyTrainer?.trainerSprite != null);
        if(CombatSystem.EnemyTrainer?.trainerSprite != null){
            enemyTrainerImage.sprite = CombatSystem.EnemyTrainer.trainerSprite;
        }
        else{
            wildMon.battleHUD.SetBattleHUD(wildMon.pokemon);
            wildMon.monSpriteObject.GetComponent<Image>().sprite = wildMon.pokemon.frontSprite;
            wildMon.monInnerMask.color = new Color(0, 0, 0, 1);
            wildMon.battleHUD.gameObject.SetActive(true);
            wildMon.monSpriteObject.SetActive(true);
        }
        //player trainer portrait slide animation
        playerPlatform.PlayAnimation();
        enemyPlatform.PlayAnimation();
    }

    public IEnumerator InitialUIAnimation(BattleTarget wildMon){
        if(CombatSystem.EnemyTrainer != null){
            //team party counter animation
            yield break;
        }
        AudioManager.Instance.PlaySoundEffect(wildMon.pokemon.pokemonDefault.cry, 0.4f);
        yield return StartCoroutine(wildMon.monInnerMask.GetComponent<SingleAnimOverride>().PlayAnimationWait(1));
    }

    public IEnumerator WriteOpeningBattleMessage(string wildMonName){
        Trainer enemy = CombatSystem.EnemyTrainer;
        string message = enemy == null
        ? "A wild " + wildMonName + " appeared!"
        : enemy.trainerTitle + " " + enemy.trainerName + " would like to battle";
        yield return StartCoroutine(battleText.WriteMessageConfirm(message));
    }

    public IEnumerator OpeningAnimationSequence(List<BattleTarget> battleTargets){
        //remove this when the proper animations are implemented
        foreach(BattleTarget b in battleTargets){
            b.battleHUD.SetBattleHUD(b.pokemon);
            b.monSpriteObject.GetComponent<Image>().sprite = b.teamBattleModifier.isPlayerTeam ? b.pokemon.backSprite : b.pokemon.frontSprite;
            b.battleHUD.gameObject.SetActive(true);
            b.monSpriteObject.SetActive(true);
        }

        if(CombatSystem.EnemyTrainer != null){
            StartCoroutine(enemyTrainerImage.GetComponent<SingleAnimOverride>().PlayAnimationWait());
            //enemy send out pokemon animations
            yield break;
        }
        //player send out pokemon animations
    }

    public IEnumerator EndTrainerBattleSequence(Trainer enemyTrainer){
        yield return StartCoroutine(battleText.WriteMessageConfirm("Player defeated " + enemyTrainer.trainerTitle + " " + enemyTrainer.trainerName));
        yield return StartCoroutine(enemyTrainerImage.GetComponent<SingleAnimOverride>().PlayAnimationWait(1));
        yield return StartCoroutine(battleText.WriteMessageConfirm(enemyTrainer.victoryMessage));
        yield return StartCoroutine(battleText.WriteMessageConfirm("Player earned $" + enemyTrainer.rewardDollars + " for winning"));
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

    public void HideActionPanel(){
        battleOptionsLayoutObject.SetActive(false);
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
