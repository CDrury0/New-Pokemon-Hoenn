using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public enum Weather { None, Rain, Sunlight, Hail, Sandstorm}
public enum SemiInvulnerable { None, Airborne, Underground, Underwater }
public enum TargetType {Self, Single, Foes, Ally, RandomFoe, All}
public class CombatSystem : MonoBehaviour
{
    public class MoveRecord{
        public Pokemon user;
        public Pokemon target;
        public GameObject moveUsed;

        public MoveRecord(Pokemon user, Pokemon target, GameObject moveUsed){
            this.user = user;
            this.target = target;
            this.moveUsed = moveUsed;
        }
    }
    public static Weather Weather {get; set;}
    public static int weatherTimer;
    public static int TurnCount {get; private set;}
    public static BattleTarget ActiveTarget {get; private set;}
    public static List<MoveRecord> MoveRecordList {get; private set;}
    public CombatScreen combatScreen;
    public MoveFunctions moveFunctions;
    public bool DoubleBattle {get; private set;}
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    private BattleTarget player1;
    private BattleTarget player2;
    private BattleTarget enemy1;
    private BattleTarget enemy2;
    public List<BattleTarget> BattleTargets {get; private set;}
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
    public void StartBattle(SerializablePokemon[] enemyPartyTemplate, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        StartCoroutine(RealStartBattle(new Party(enemyPartyTemplate), trainerBattle, doubleBattle, enemyAI));
    }

    public void StartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){  //no not use this except with battleTestMenu
        StartCoroutine(RealStartBattle(enemyParty, trainerBattle, doubleBattle, enemyAI));
    }

    private IEnumerator RealStartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        TurnCount = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        MoveRecordList = new List<MoveRecord>();
        playerParty = PlayerParty.Instance.playerParty;
        this.DoubleBattle = doubleBattle;
        this.enemyParty = enemyParty;
        this.enemyAI = enemyAI;

        combatScreen.SetBattleSpriteFormat(doubleBattle);

        TeamBattleModifier playerTeamModifier = new TeamBattleModifier(trainerBattle, true);
        TeamBattleModifier enemyTeamModifier = new TeamBattleModifier(trainerBattle, false);
        player1 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), playerParty.GetFirstAvailable(), combatScreen.player1hud, combatScreen.player1Object);
        player1.pokemon.inBattle = true;
        enemy1 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), this.enemyParty.GetFirstAvailable(), combatScreen.enemy1hud, combatScreen.enemy1Object);
        enemy1.pokemon.inBattle = true;

        player2 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), playerParty.GetFirstAvailable(), combatScreen.player2hud, combatScreen.player2Object);
        enemy2 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), this.enemyParty.GetFirstAvailable(), combatScreen.enemy2hud, combatScreen.enemy2Object);

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

        BattleTargets = new List<BattleTarget>(){player1, enemy1, player2, enemy2};
        BattleTargets.RemoveAll(b => b.pokemon == null);
        
        //replace with proper animations
        combatScreen.SetStartingGraphics(BattleTargets);
        combatScreen.gameObject.SetActive(true);

        //check tag-in effects like intimidate, trace, etc.

        StartCoroutine(GetTurnActions());
        yield break;
    }

    public Pokemon[] GetTeamParty(BattleTarget whoseTeam){
        return whoseTeam.teamBattleModifier.isPlayerTeam ? playerParty.party : enemyParty.party;
    }

    private IEnumerator GetTurnActions(){
        combatScreen.battleText.gameObject.SetActive(false);

        foreach(BattleTarget b in BattleTargets){
            ActiveTarget = b;
            if(!b.teamBattleModifier.isPlayerTeam){
                //get enemyAI selection
                List<GameObject> possibleMoves = new List<GameObject>(ActiveTarget.pokemon.moves);
                possibleMoves.RemoveAll(move => move == null);
                ActiveTarget.turnAction = ActiveTarget.pokemon.moves[UnityEngine.Random.Range(0, ActiveTarget.pokemon.moves.Count)];
                if(moveFunctions.MustChooseTarget(ActiveTarget.turnAction.GetComponent<MoveData>().targetType, ActiveTarget, BattleTargets, DoubleBattle)){
                    ActiveTarget.individualBattleModifier.targets = new List<BattleTarget>(){player1};
                }
            }
            else{
                //if GetRequiredAction != null (player is locked into action from previous turn), make that the action and do not allow manual selection

                //else allow player to select an action manually
                combatScreen.SetActionPromptText();
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
        if(moveFunctions.MustChooseTarget(ActiveTarget.pokemon.moves[whichMove].GetComponent<MoveData>().targetType, ActiveTarget, BattleTargets, DoubleBattle)){
            EnableTargetButtons();
        }
        else{
            Proceed = true;
        }
    }

    private void EnableTargetButtons(){
        combatScreen.battleText.WriteMessageInstant("Select a target");
        combatScreen.targetBackButton.SetActive(true);
        foreach(BattleTarget b in BattleTargets){
            if(b != ActiveTarget){
                b.monSpriteObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void TargetButtonFunction(GameObject spriteClicked){
        combatScreen.HideTargetButtons();
        ActiveTarget.individualBattleModifier.targets = new List<BattleTarget>(){BattleTargets.First(b => b.monSpriteObject == spriteClicked)};
        Proceed = true;
    }

    public class WrappedBool{ public bool failed;}

    public IEnumerator UseMove(BattleTarget user, GameObject move, bool calledFromOtherMove, bool doDeductPP){
        if(!calledFromOtherMove){
            WrappedBool moveFailed = new WrappedBool();
            yield return StartCoroutine(moveFunctions.CheckMoveFailedToBeUsed(moveFailed, user));
            //check for any valid target
            if(moveFailed.failed){
                yield break;
            }
        }

        MoveData moveData = move.GetComponent<MoveData>();
        if(doDeductPP){
            moveFunctions.DeductPP(user);
        }
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " used " + moveData.moveName));
    
        int targetCount = user.individualBattleModifier.targets.Count;  //must save value of target count or calling moves may activate multiple times during double battles
        for(int j = 0; j < targetCount; j++){
            MoveRecordList.Add(new MoveRecord(user.pokemon, user.individualBattleModifier.targets[j].pokemon, user.turnAction));

            WrappedBool moveFailed = new WrappedBool();
            yield return StartCoroutine(moveFunctions.CheckMoveFailedAgainstTarget(moveFailed, move, user, user.individualBattleModifier.targets[j]));
            if(moveFailed.failed){
                continue;
            }

            //play move animation only the first time it is successfully used on a target
            foreach(MoveEffect effect in move.GetComponents<MoveEffect>()){
                BattleTarget target = effect.applyToSelf ? user : user.individualBattleModifier.targets[j];
                if(effect is ICheckMoveEffectFail){
                    ICheckMoveEffectFail effectThatMayFail = (ICheckMoveEffectFail)effect;
                    if(effectThatMayFail.CheckMoveEffectFail(user, target, moveData)){
                        continue;
                    }
                }
                yield return StartCoroutine(effect.DoEffect(user, target, moveData));
            }
        }
    }

    private IEnumerator BattleTurn(){
        //increment turn count, reset damage taken this turn, destroy instantiated turnAction gameobjects, other cleanup things
        
        List<BattleTarget> orderedUsers = moveFunctions.GetTurnOrder(BattleTargets);

        for(int i = 0; i < orderedUsers.Count; i++){
            BattleTarget user = orderedUsers[i];
            GameObject action = Instantiate(user.turnAction);

            if(action.CompareTag("Move")){
                yield return StartCoroutine(UseMove(user, action, false, true));
                
                //effects after move usage?
            }
            //else if switch, item, etc.
        }

        yield return StartCoroutine(moveFunctions.EndOfTurnEffects(orderedUsers));
        //end of turn effects
        //check for fainting, etc.

        StartCoroutine(GetTurnActions());
    }

    private void EndBattle(){
        foreach(Pokemon p in playerParty.party){
            p.inBattle = false;
        }

        GameObject[] instantiatedMoves = GameObject.FindGameObjectsWithTag("Move");
        foreach(GameObject oldTurnAction in instantiatedMoves){
            Destroy(oldTurnAction);
        }
    }
}