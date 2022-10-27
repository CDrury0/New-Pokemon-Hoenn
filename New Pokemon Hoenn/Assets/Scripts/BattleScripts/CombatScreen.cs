using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatScreen : MonoBehaviour
{
    [SerializeField] private RectTransform playerSingleTransform;
    [SerializeField] private RectTransform enemySingleTransform;
    [SerializeField] private RectTransform playerDoubleTransform;
    [SerializeField] private RectTransform enemyDoubleTransform;
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
}
