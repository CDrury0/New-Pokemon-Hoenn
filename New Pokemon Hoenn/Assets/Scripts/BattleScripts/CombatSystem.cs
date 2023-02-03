using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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
    public PartyMenu partyMenu;
    public GameObject struggle;
    public GameObject switchAction;
    public bool DoubleBattle {get; private set;}
    private Party playerParty;
    private Party enemyParty;
    private EnemyAI enemyAI;
    private BattleTarget player1;
    private BattleTarget player2;
    private BattleTarget enemy1;
    private BattleTarget enemy2;
    private List<BattleTarget> referenceBattleTargets;
    public static List<BattleTarget> BattleTargets {get; private set;}
    private List<Pokemon> expParticipants;
    private List<BattleTarget> turnOrder;
    private static bool _proceed;
    public static bool Proceed {
        private get{
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

        //replace Pokemon argument with null, followed by SendOutPokemon using GetFirstAvailable
        player1 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), playerParty.GetFirstAvailable(), combatScreen.player1hud, combatScreen.player1Object);
        enemy1 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), this.enemyParty.GetFirstAvailable(), combatScreen.enemy1hud, combatScreen.enemy1Object);

        player2 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(), null, combatScreen.player2hud, combatScreen.player2Object);
        enemy2 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(), null, combatScreen.enemy2hud, combatScreen.enemy2Object);

        referenceBattleTargets = new List<BattleTarget>(){player1, enemy1};
        if(doubleBattle){
            player2.pokemon = playerParty.GetFirstAvailable();
            enemy2.pokemon = enemyParty.GetFirstAvailable();
            referenceBattleTargets.AddRange(new List<BattleTarget>(){player2, enemy2});
        }
        
        BattleTargets = new List<BattleTarget>(referenceBattleTargets.FindAll(b => b.pokemon != null));
        
        //replace with proper animations
        combatScreen.SetStartingGraphics(BattleTargets);
        combatScreen.gameObject.SetActive(true);

        //check tag-in effects like intimidate, trace, etc.

        StartCoroutine(GetTurnActions());
        yield break;
    }

    public static bool ActiveTargetCanSwitchOut(){
        return ActiveTarget.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyBind || e.effect is ApplyTrap || e.effect is ApplyIngrain) == null;      
    }

    public Pokemon[] GetTeamParty(BattleTarget whoseTeam){
        return whoseTeam.teamBattleModifier.isPlayerTeam ? playerParty.party : enemyParty.party;
    }

    public void OpenPartyMenu(){
        ActiveTarget.turnAction = switchAction;
        partyMenu.OpenParty(true);
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
                    //only do self-applied effects one time regardless of the number of targets
                    if(effect.applyToSelf && j + 1 != user.individualBattleModifier.targets.Count){
                        continue;
                    }
                    if(effect is ICheckMoveEffectFail){
                        ICheckMoveEffectFail effectThatMayFail = (ICheckMoveEffectFail)effect;
                        if(effectThatMayFail.CheckMoveEffectFail(user, target, moveData)){
                            continue;
                        }
                    }
                    yield return StartCoroutine(effect.DoEffect(user, target, moveData));
                }
            }

            if(user.individualBattleModifier.targets.Count > 0){
                MoveRecordList.AddRecord(user.pokemon, user.individualBattleModifier.targets[j].pokemon, user.turnAction);
            }

            if(MoveFunctions.IsChargingTurn(move)){
                break;
            }
        }
    }

    private IEnumerator BattleTurn(){
        //increment turn count, reset damage taken this turn, destroy instantiated turnAction gameobjects, other cleanup things
        
        BattleTargets = moveFunctions.GetTurnOrder(BattleTargets);

        for(int i = 0; i < BattleTargets.Count; i++){
            ActiveTarget = BattleTargets[i];
            GameObject action = Instantiate(ActiveTarget.turnAction);

            AppliedEffectInfo onFaintInfo = ActiveTarget.individualBattleModifier.appliedEffects.Find(effectInfo => effectInfo.effect is ApplyOnFaintEffect);
            if(onFaintInfo != null){
                onFaintInfo.effect.RemoveEffect(ActiveTarget, onFaintInfo);
            }

            if(action.CompareTag("Move")){
                PreMoveEffects(ActiveTarget, ActiveTarget.turnAction);

                yield return StartCoroutine(UseMove(ActiveTarget, action, false, true));
                
                PostMoveEffects(ActiveTarget, ActiveTarget.turnAction);
            }

            else if(action.CompareTag("Switch")){
                yield return StartCoroutine(action.GetComponent<MoveEffect>().DoEffect(ActiveTarget, null, null));
            }

            yield return StartCoroutine(HandleFaint());
        }

        yield return StartCoroutine(moveFunctions.EndOfTurnEffects(BattleTargets));

        BattleTargets = referenceBattleTargets.FindAll(b => BattleTargets.Contains(b));

        yield return StartCoroutine(ReplaceFaintedTargets());

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

    public IEnumerator HandleFaint(){
        foreach(BattleTarget b in BattleTargets){
            if(b.pokemon.CurrentHealth == 0){
                b.pokemon.primaryStatus = PrimaryStatus.Fainted;
                b.individualBattleModifier = new IndividualBattleModifier();
                b.pokemon.inBattle = false;
                //animation for fainting, remove direct sprite object change
                b.monSpriteObject.SetActive(false);
                b.battleHUD.gameObject.SetActive(false);
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(b.GetName() + " fainted"));
                //do xp here if fainted mon is opponent
            }
        }

        BattleTargets.RemoveAll(b => b.pokemon.primaryStatus == PrimaryStatus.Fainted);
    }

    public IEnumerator ReplaceFaintedTargets(){
        List<BattleTarget> targetsToReplace = new List<BattleTarget>(referenceBattleTargets.FindAll(b => !BattleTargets.Contains(b)));
        foreach(BattleTarget needsReplaced in targetsToReplace){
            ActiveTarget = needsReplaced;
            if(!ActiveTarget.teamBattleModifier.isPlayerTeam){
                ActiveTarget.individualBattleModifier.switchingIn = enemyAI.SelectNextPokemon(enemyParty);
            }
            else{
                if(playerParty.HasAvailableFighter()){
                    partyMenu.OpenParty(false);
                }
                else{
                    Proceed = true;
                }
                yield return new WaitUntil(() => Proceed);
            }
        }
        foreach(BattleTarget needsReplaced in targetsToReplace){
            if(needsReplaced.individualBattleModifier.switchingIn != null){
                yield return StartCoroutine(SendOutPokemon(needsReplaced));
                BattleTargets.Insert(referenceBattleTargets.IndexOf(needsReplaced), needsReplaced);
            }
        }
    }

    public IEnumerator SwitchPokemon(BattleTarget replacing){
        //play withdraw animation
        if(replacing.individualBattleModifier.switchingIn == null){
            if(!replacing.teamBattleModifier.isPlayerTeam){
                replacing.individualBattleModifier.switchingIn = enemyAI.SelectNextPokemon(enemyParty);
            }
            else{
                partyMenu.OpenParty(false);
                yield return new WaitUntil(() => Proceed);
            }
        }
        yield return StartCoroutine(SendOutPokemon(replacing));
    }

    private IEnumerator SendOutPokemon(BattleTarget replacing){
        replacing.pokemon.inBattle = false;
        MoveRecordList.RemoveAllRecordsOfUser(replacing.pokemon);

        replacing.pokemon = replacing.individualBattleModifier.switchingIn;
        replacing.pokemon.inBattle = true;

        //account for baton pass
        replacing.individualBattleModifier = new IndividualBattleModifier();
        
        replacing.battleHUD.SetBattleHUD(replacing.pokemon);
        replacing.monSpriteObject.GetComponent<Image>().sprite = replacing.teamBattleModifier.isPlayerTeam ? replacing.pokemon.backSprite : replacing.pokemon.frontSprite;
        //replace setActives later
        replacing.monSpriteObject.SetActive(true);
        replacing.battleHUD.gameObject.SetActive(true);

        string message = replacing.teamBattleModifier.isPlayerTeam ? "Go " + replacing.pokemon.nickName + "!" : "Enemy sent out " + replacing.pokemon.nickName;
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(message));
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
