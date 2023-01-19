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
    public static bool BattleActive {get; private set;}
    public static Weather Weather {get; set;}
    public static int weatherTimer;
    public static int TurnCount {get; private set;}
    public static BattleTarget ActiveTarget {get; private set;}
    public static MoveRecordList MoveRecordList {get; private set;}
    public CombatScreen combatScreen;
    public MoveFunctions moveFunctions;
    public GameObject struggle;
    public GameObject switchPokemon;
    public bool DoubleBattle {get; private set;}
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    private BattleTarget player1;
    private BattleTarget player2;
    private BattleTarget enemy1;
    private BattleTarget enemy2;
    private List<BattleTarget> referenceBattleTargets;
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
    //make this accept a Trainer object that contains SerializablePokemon[], EnemyAI, and other data
    public void StartBattle(SerializablePokemon[] enemyPartyTemplate, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        StartCoroutine(RealStartBattle(new Party(enemyPartyTemplate), trainerBattle, doubleBattle, enemyAI));
    }

    public void StartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){  //do not use this except with battleTestMenu
        StartCoroutine(RealStartBattle(enemyParty, trainerBattle, doubleBattle, enemyAI));
    }

    private IEnumerator RealStartBattle(Party enemyParty, bool trainerBattle, bool doubleBattle, EnemyAI enemyAI){
        BattleActive = true;
        TurnCount = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        MoveRecordList = new MoveRecordList();
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

        referenceBattleTargets = new List<BattleTarget>(){player1, enemy1, player2, enemy2};
        BattleTargets = new List<BattleTarget>(referenceBattleTargets.FindAll(b => b.pokemon != null));
        
        //replace with proper animations
        combatScreen.SetStartingGraphics(BattleTargets);
        combatScreen.gameObject.SetActive(true);

        //check tag-in effects like intimidate, trace, etc.

        StartCoroutine(GetTurnActions());
        yield break;
    }

    public static bool ActiveTargetCanSwitch(){
        return ActiveTarget.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyBind || e.effect is ApplyTrap || e.effect is ApplyIngrain) == null;      
    }

    public Pokemon[] GetTeamParty(BattleTarget whoseTeam){
        return whoseTeam.teamBattleModifier.isPlayerTeam ? playerParty.party : enemyParty.party;
    }

    private IEnumerator GetTurnActions(){
        combatScreen.battleText.gameObject.SetActive(false);

        foreach(BattleTarget b in BattleTargets){
            ActiveTarget = b;
            if(!ActiveTarget.teamBattleModifier.isPlayerTeam){
                //get enemyAI action
                enemyAI.ChooseAction(ActiveTarget);
            }
            else{
                if(!moveFunctions.LockedIntoAction(ActiveTarget)){
                    combatScreen.SetActionPromptText();
                    combatScreen.battleOptionsLayoutObject.SetActive(true);
                    yield return new WaitUntil(() => Proceed);
                }
            }
        }
        StartCoroutine(BattleTurn());
    }

    public void FightButtonFunction(){
        combatScreen.battleOptionsLayoutObject.SetActive(false);

        List<GameObject> unusableMoves = GetAllUnusableMoves(ActiveTarget);

        bool[] isSelectables = new bool[ActiveTarget.pokemon.moves.Capacity];
        for(int i = 0; i < ActiveTarget.pokemon.moves.Count; i++){
            GameObject move = ActiveTarget.pokemon.moves[i];
            isSelectables[i] = !unusableMoves.Contains(move);
        }

        //if choosing fight forces move selection (i.e. all moves are unselectable), force select struggle
        if(isSelectables.Where(b => b == false).ToArray().Length == ActiveTarget.pokemon.moves.Capacity){
            ActiveTarget.turnAction = struggle;
            moveFunctions.MustChooseTarget(TargetType.RandomFoe, ActiveTarget);
            Proceed =  true;
        }
        else{
            combatScreen.ShowMoveButtons(ActiveTarget.pokemon, isSelectables);
        }
    }

    public void MoveButtonFunction(int whichMove){
        ActiveTarget.turnAction = ActiveTarget.pokemon.moves[whichMove];
        combatScreen.HideMoveButtons();
        if(moveFunctions.MustChooseTarget(ActiveTarget.turnAction.GetComponent<MoveData>().targetType, ActiveTarget)){
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

    public void VerifyMoveTarget(BattleTarget user, GameObject move){
        if(move.GetComponent<CounterDamage>() != null || (move.GetComponent<DirectDamage>() != null && move.GetComponent<DirectDamage>().bideRelease)){
            user.individualBattleModifier.targets = new List<BattleTarget>(){user.individualBattleModifier.lastOneToDealDamage};
        }

        //correct targeting fainted pokemon in double battles
    }

    public List<GameObject> GetAllUnusableMoves(BattleTarget user){
        List<GameObject> unusableMoves = new List<GameObject>();
        foreach(AppliedEffectInfo effectInfo in user.individualBattleModifier.appliedEffects.FindAll(e => e.effect is ICheckMoveSelectable)){
            ICheckMoveSelectable effectProhibitingMoves = (ICheckMoveSelectable)effectInfo.effect;
            unusableMoves.AddRange(effectProhibitingMoves.GetUnusableMoves(user));
        }
        unusableMoves.Add(null);
        unusableMoves.AddRange(GetImprisonMoves(user));
        unusableMoves.AddRange(user.pokemon.moves.FindAll(move => user.pokemon.movePP[user.pokemon.moves.IndexOf(move)] == 0));
        return unusableMoves;
    }

    private List<GameObject> GetImprisonMoves(BattleTarget user){
        List<BattleTarget> imprisonUsers = BattleTargets.FindAll(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam && b.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyImprison) != null);
        List<GameObject> imprisonedMoves = new List<GameObject>();
        foreach(BattleTarget b in imprisonUsers){
            imprisonedMoves.AddRange(b.pokemon.moves);
        }
        return imprisonedMoves;
    }

    public class WrappedBool{ public bool failed;}

    public IEnumerator UseMove(BattleTarget user, GameObject move, bool calledFromOtherMove, bool doDeductPP){
        if(!calledFromOtherMove){
            WrappedBool moveFailed = new WrappedBool();
            yield return StartCoroutine(moveFunctions.CheckMoveFailedToBeUsed(moveFailed, user));
            if(moveFailed.failed){
                user.individualBattleModifier.multiTurnInfo = null;
                yield break;
            }
        }

        MoveData moveData = move.GetComponent<MoveData>();
        if(doDeductPP){
            moveFunctions.DeductPP(user);
        }
        
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " used " + moveData.moveName));
    
        int targetCount = user.individualBattleModifier.targets.Count;  //must save value of target count or calling moves like mirror move may activate multiple times during double battles
        for(int j = 0; j < targetCount; j++){
            WrappedBool moveFailed = new WrappedBool();
            yield return StartCoroutine(moveFunctions.CheckMoveFailedAgainstTarget(moveFailed, move, user, user.individualBattleModifier.targets[j]));

            //play move animation only the first time it is successfully used on a target
            if(!moveFailed.failed){
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

            if(user.individualBattleModifier.targets[j] != null){
                MoveRecordList.AddRecord(user.pokemon, user.individualBattleModifier.targets[j].pokemon, user.turnAction);
            }

            if(moveFunctions.IsChargingTurn(move)){
                break;
            }
        }
    }

    private IEnumerator BattleTurn(){
        //increment turn count, reset damage taken this turn, destroy instantiated turnAction gameobjects, other cleanup things
        
        List<BattleTarget> orderedUsers = moveFunctions.GetTurnOrder(BattleTargets);

        for(int i = 0; i < orderedUsers.Count; i++){
            BattleTarget user = orderedUsers[i];
            GameObject action = Instantiate(user.turnAction);

            AppliedEffectInfo onFaintInfo = user.individualBattleModifier.appliedEffects.Find(effectInfo => effectInfo.effect is ApplyOnFaintEffect);
            if(onFaintInfo != null){
                onFaintInfo.effect.RemoveEffect(user, onFaintInfo);
            }

            if(action.CompareTag("Move")){
                PreMoveEffects(user, user.turnAction);

                yield return StartCoroutine(UseMove(user, action, false, true));
                
                PostMoveEffects(user, user.turnAction);
            }
            //else if switch, item, etc.
            //do moveFunctions.AppliedEffectOfType<ApplyOnFaintEffect>() when handling fainting
        }

        yield return StartCoroutine(moveFunctions.EndOfTurnEffects(orderedUsers));

        StartCoroutine(GetTurnActions());
    }

    private void PreMoveEffects(BattleTarget user, GameObject moveUsed){
        if(moveUsed.GetComponent<ApplyCurse>() != null && user.pokemon.IsThisType(StatLib.Type.Ghost)){
            moveFunctions.MustChooseTarget(TargetType.RandomFoe, user);
        }

        VerifyMoveTarget(user, moveUsed);

        GameObject usedLastTurn = MoveRecordList.FindRecordLastUsedBy(user.pokemon)?.moveUsed;
        if(usedLastTurn != null && usedLastTurn != user.turnAction){
            user.individualBattleModifier.consecutiveMoveCounter = 0;
        }
        else{
            user.individualBattleModifier.consecutiveMoveCounter++;
        }
    }

    private void PostMoveEffects(BattleTarget user, GameObject moveUsed){
        if(moveUsed.GetComponent<MultiTurnEffect>() != null && moveUsed.GetComponent<MultiTurnEffect>().givesSemiInvulnerable == SemiInvulnerable.None){
            user.individualBattleModifier.semiInvulnerable = SemiInvulnerable.None;
        }
    }

    private void EndBattle(){
        BattleActive = false;

        foreach(Pokemon p in playerParty.party){
            p.inBattle = false;
        }

        GameObject[] instantiatedMoves = GameObject.FindGameObjectsWithTag("Move");
        foreach(GameObject oldTurnAction in instantiatedMoves){
            Destroy(oldTurnAction);
        }
    }
}