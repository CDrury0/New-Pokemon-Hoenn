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
    [SerializeField] private GameObject battlePartyScreen;
    [SerializeField] private GameObject battlePartyScreenFaint;
    public GameObject battleOptionsLayoutObject;
    public GameObject moveButtonLayoutObject;
    public GameObject moveBackButton;
    public GameObject targetBackButton;
    public Color emptyColor;
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
        player1hud.SlideOut();
        player2hud.SlideOut();
        enemy1hud.SlideOut();
        enemy2hud.SlideOut();

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
            wildMon.monSpriteObject.SetActive(true);
            wildMon.pokemon.inBattle = true;
        }
        //player trainer portrait slide animation
        playerPlatform.PlayAnimation();
        enemyPlatform.PlayAnimation();
    }

    private IEnumerator EncounterAnimation(BattleTarget wildMon){
        if(CombatSystem.EnemyTrainer != null){
            //team party counter animation
            yield break;
        }
        AudioManager.Instance.PlaySoundEffect(wildMon.pokemon.pokemonDefault.cry, 0.4f);
        yield return StartCoroutine(wildMon.monInnerMask.GetComponent<SingleAnimOverride>().PlayAnimationWait(1));
        wildMon.battleHUD.SlideIn();
    }

    private IEnumerator WriteOpeningBattleMessage(string wildMonName){
        Trainer enemy = CombatSystem.EnemyTrainer;
        string message = enemy == null
        ? "A wild " + wildMonName + " appeared!"
        : enemy.GetName() + " would like to battle";
        yield return StartCoroutine(battleText.WriteMessageConfirm(message));
    }

    public IEnumerator OpeningAnimationSequence(List<BattleTarget> battleTargets, BattleTarget wildMon){
        yield return StartCoroutine(barsOpening.TransitionLogic());
        yield return StartCoroutine(EncounterAnimation(wildMon));
        yield return StartCoroutine(WriteOpeningBattleMessage(wildMon.pokemon.pokemonName));

        if(CombatSystem.EnemyTrainer != null){
            enemyTrainerImage.GetComponent<SingleAnimOverride>().PlayAnimation();
            List<BattleTarget> enemyMons = battleTargets.FindAll(b => !b.teamBattleModifier.isPlayerTeam);
            TempSendOutAnimation(enemyMons);
        }

        List<BattleTarget> playerMons = battleTargets.FindAll(b => b.teamBattleModifier.isPlayerTeam);
        TempSendOutAnimation(playerMons);
    }

    private void TempSendOutAnimation(List<BattleTarget> teamMons){
        foreach(BattleTarget mon in teamMons){
            //play send out animation, and wait slightly
            mon.battleHUD.SetBattleHUD(mon.pokemon);
            mon.monSpriteObject.GetComponent<Image>().sprite = mon.teamBattleModifier.isPlayerTeam ? mon.pokemon.backSprite : mon.pokemon.frontSprite;
            mon.battleHUD.SlideIn();
            mon.monSpriteObject.SetActive(true);
        }
    }

    public IEnumerator EndTrainerBattleSequence(Trainer enemyTrainer){
        yield return StartCoroutine(battleText.WriteMessageConfirm("Player defeated " + enemyTrainer.GetName()));
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
            buttonImage.color = emptyColor;
        }
        else{
            MoveData moveData = pokemon.moves[whichMove].GetComponent<MoveData>();
            buttonText.text = moveData.moveName + " " + pokemon.movePP[whichMove] + "/" + pokemon.moveMaxPP[whichMove];
            buttonImage.color = moveData.GetEffectiveMoveType(pokemon).typeColor;
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

    public PartyMenu SetPartyScreen(bool allowClose = true, string promptMessage = null){
        return SetupPartyMenu(battlePartyScreen, allowClose, promptMessage);
    }

    public PartyMenu SetPartyScreenFaint(bool allowClose = true, string promptMessage = null){
        return SetupPartyMenu(battlePartyScreenFaint, allowClose, promptMessage);
    }

    private PartyMenu SetupPartyMenu(GameObject menuPrefab, bool allowClose, string promptMessage){
        PartyMenu partyMenuInstance = Instantiate(menuPrefab).GetComponentInChildren<PartyMenu>();
        partyMenuInstance.SetAllowClose(allowClose);
        if(promptMessage != null){
            partyMenuInstance.WriteTextPrompt(promptMessage);
        }
        return partyMenuInstance;
    }
}
