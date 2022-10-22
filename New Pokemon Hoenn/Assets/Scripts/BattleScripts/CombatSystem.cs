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
    private int weatherTimer;
    private bool doubleBattle;
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    [SerializeField] private BattleTarget player1;
    [SerializeField] private BattleTarget player2;
    [SerializeField] private BattleTarget enemy1;
    [SerializeField] private BattleTarget enemy2;
    private List<BattleTarget> participants;
    private List<BattleTarget> turnOrder;

    public void StartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        TurnCount = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        playerParty = PlayerParty.Instance.playerParty;
        this.doubleBattle = doubleBattle;
        this.enemyParty = new Party(enemyParty);
        this.enemyAI = enemyAI;

        player1 = new BattleTarget(new TeamBattleModifier(trainerBattle, true), new IndividualBattleModifier(), playerParty.GetFirstAvailable());
        enemy1 = new BattleTarget(new TeamBattleModifier(trainerBattle, false), new IndividualBattleModifier(), enemyParty.GetFirstAvailable());
    }

    public void EndBattle(){
        
    }
}