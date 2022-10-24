using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum Weather { None, Rain, Sunlight, Hail, Sandstorm}
public enum SemiInvulnerable { None, Airborne, Underground, Underwater }
public enum TargetType {Self, Single, Foes, Ally, RandomFoe, All}
public class CombatSystem : MonoBehaviour
{
    public static Weather Weather {get; private set;}
    public static int TurnCount {get; private set;}
    public GameObject battleScreenTempObject;
    public GameObject player2SpriteTemp;
    public GameObject enemy2SpriteTemp;
    public GameObject player1SpriteTemp;
    public GameObject enemy1SpriteTemp;
    public RectTransform playerDoubleTransform;
    public RectTransform playerSingleTransform;
    public RectTransform enemyDoubleTransform;
    public RectTransform enemySingleTransform;
    private int weatherTimer;
    private bool doubleBattle;
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    [SerializeField] private BattleHUD player1hud;
    [SerializeField] private BattleHUD player2hud;
    [SerializeField] private BattleHUD enemy1hud;
    [SerializeField] private BattleHUD enemy2hud;
    //add references to pokemon sprite animator?
    private BattleTarget player1;
    private BattleTarget player2;
    private BattleTarget enemy1;
    private BattleTarget enemy2;
    private List<BattleTarget> monsInBattle;
    private List<Pokemon> expParticipants;
    private List<BattleTarget> turnOrder;

    public void StartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        TurnCount = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        playerParty = PlayerParty.Instance.playerParty;
        this.doubleBattle = doubleBattle;
        this.enemyParty = new Party(enemyParty);
        this.enemyAI = enemyAI;

        player1 = new BattleTarget(new TeamBattleModifier(trainerBattle, true), new IndividualBattleModifier(), playerParty.GetFirstAvailable(), player1hud);
        enemy1 = new BattleTarget(new TeamBattleModifier(trainerBattle, false), new IndividualBattleModifier(), enemyParty.GetFirstAvailable(), enemy1hud);

        player1SpriteTemp.GetComponent<RectTransform>().anchoredPosition = playerSingleTransform.anchoredPosition;
        enemy1SpriteTemp.GetComponent<RectTransform>().anchoredPosition = enemySingleTransform.anchoredPosition;

        player2SpriteTemp.SetActive(false);
        enemy2SpriteTemp.SetActive(false);
        player2hud.gameObject.SetActive(false);
        enemy2hud.gameObject.SetActive(false);

        if(doubleBattle){
            player1SpriteTemp.GetComponent<RectTransform>().anchoredPosition = playerDoubleTransform.anchoredPosition;
            enemy1SpriteTemp.GetComponent<RectTransform>().anchoredPosition = enemyDoubleTransform.anchoredPosition;

            if(playerParty.GetFirstAvailable() != null){
                player2hud.gameObject.SetActive(true);
                player2SpriteTemp.gameObject.SetActive(true);
            }

            if(enemyParty.GetFirstAvailable() != null){
                enemy2hud.gameObject.SetActive(true);
                enemy2SpriteTemp.gameObject.SetActive(true);
            }
        }

        battleScreenTempObject.SetActive(true);
    }

    private void EndBattle(){
        foreach(Pokemon p in playerParty.party){
            p.inBattle = false;
        }
    }
}