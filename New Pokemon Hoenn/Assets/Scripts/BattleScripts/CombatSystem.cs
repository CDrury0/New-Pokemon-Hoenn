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
    public CombatScreen combatScreen;
    private int weatherTimer;
    private bool doubleBattle;
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    private BattleTarget player1;
    private BattleTarget player2;
    private BattleTarget enemy1;
    private BattleTarget enemy2;
    private List<BattleTarget> battleTargets;
    private List<Pokemon> expParticipants;
    private List<BattleTarget> turnOrder;

    //must start the coroutine from this monobehaviour so it doesn't matter if originating gameobject is set to inactive
    public void StartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        StartCoroutine(RealStartBattle(enemyParty, trainerBattle, doubleBattle, enemyAI));
    }

    private IEnumerator RealStartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        TurnCount = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        playerParty = PlayerParty.Instance.playerParty;
        this.doubleBattle = doubleBattle;
        this.enemyParty = new Party(enemyParty);
        this.enemyAI = enemyAI;

        combatScreen.SetBattleSpriteFormat(doubleBattle);

        //add created battletargets to monsInBattle, e.g. for each in monsInBattle, battleTarget.battleHud.setActive(true);
        TeamBattleModifier playerTeamModifier = new TeamBattleModifier(trainerBattle, true);
        TeamBattleModifier enemyTeamModifier = new TeamBattleModifier(trainerBattle, false);
        player1 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), playerParty.GetFirstAvailable(), combatScreen.player1hud, combatScreen.player1Object);
        player1.pokemon.inBattle = true;
        enemy1 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), enemyParty.GetFirstAvailable(), combatScreen.enemy1hud, combatScreen.enemy1Object);
        enemy1.pokemon.inBattle = true;

        player2 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), playerParty.GetFirstAvailable(), combatScreen.player2hud, combatScreen.player2Object);
        enemy2 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), enemyParty.GetFirstAvailable(), combatScreen.enemy2hud, combatScreen.enemy2Object);

        if(doubleBattle){
            if(player2.pokemon != null){
                player2.pokemon.inBattle = true;
            }
            if(enemy2.pokemon != null){
                enemy2.pokemon.inBattle = true;
            }
        }
        else{
            player2.pokemon = null;
            enemy2.pokemon = null;
        }

        battleTargets = new List<BattleTarget>(){player1, enemy1, player2, enemy2};
        battleTargets.RemoveAll(b => b.pokemon == null);
        
        //replace with proper animations
        combatScreen.SetStartingGraphics(battleTargets);
        combatScreen.gameObject.SetActive(true);

        //check tag-in effects like intimidate, trace, etc.

        
        yield break;
    }

    

    private void EndBattle(){
        foreach(Pokemon p in playerParty.party){
            p.inBattle = false;
        }
    }
}