using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public enum Weather { None, Rain, Sunlight, Hail, Sandstorm}
public enum SemiInvulnerable { None, Airborne, Underground, Underwater }
public enum TargetType {Self, Single, Foes, Ally, RandomFoe, All}
public class CombatSystem : MonoBehaviour
{
    public static Weather Weather {get; private set;}
    public static int TurnCount {get; private set;}
    public static BattleTarget ActiveTarget {get; private set;}
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
    private bool _proceed;
    private bool Proceed {
        get{
            if(_proceed){
                _proceed = false;
                return true;
            }
            return false;
        }
        set{
            _proceed = value;
        }
    }

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

        StartCoroutine(GetTurnActions());
        yield break;
    }

    private IEnumerator GetTurnActions(){
        foreach(BattleTarget b in battleTargets){
            ActiveTarget = b;
            if(!b.teamBattleModifier.isPlayerTeam){
                //get enemyAI selection
                ActiveTarget.turnAction = ActiveTarget.pokemon.moves[0];
                ActiveTarget.individualBattleModifier.targets = new List<BattleTarget>(){player1};
            }
            else{
                //if GetRequiredAction != null (player is locked into action from previous turn), make that the action and do not allow manual selection

                //else allow player to select an action manually
                combatScreen.battleOptionsLayoutObject.SetActive(true);
                yield return new WaitUntil(() => Proceed);
            }
        }
        StartCoroutine(BattleTurn());
    }

    public void FightButtonFunction(){
        //if choosing fight forces move selection (struggle, etc.)

        //else
        combatScreen.battleOptionsLayoutObject.SetActive(false);
        combatScreen.ShowMoveButtons(ActiveTarget);
    }

    public void MoveButtonFunction(int whichMove){
        ActiveTarget.turnAction = ActiveTarget.pokemon.moves[whichMove];
        combatScreen.HideMoveButtons();
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(ActiveTarget.pokemon.moves[whichMove].GetComponent<MoveData>().targetType, ActiveTarget, battleTargets, doubleBattle)){
            EnableTargetButtons();
        }
        else{
            Proceed = true;
        }
    }

    private void EnableTargetButtons(){
        combatScreen.battleText.WriteMessageInstant("Select a target");
        combatScreen.targetBackButton.SetActive(true);
        foreach(BattleTarget b in battleTargets){
            if(b != ActiveTarget){
                b.monSpriteObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void TargetButtonFunction(GameObject spriteClicked){
        combatScreen.HideTargetButtons();
        ActiveTarget.individualBattleModifier.targets = new List<BattleTarget>(){battleTargets.First(b => b.monSpriteObject == spriteClicked)};
        Proceed = true;
    }

    private IEnumerator BattleTurn(){
        //increment turn count, reset damage taken this turn, other cleanup things

        List<int> turnOrder = CombatLib.Instance.moveFunctions.GetTurnOrder(battleTargets);
        //use the value of each element of turnOrder as the index of battleTargets (for(int i = 0...){battleTargets[turnOrder[i]].DoMove()})
        yield break;
    }

    private void EndBattle(){
        foreach(Pokemon p in playerParty.party){
            p.inBattle = false;
        }
    }
}