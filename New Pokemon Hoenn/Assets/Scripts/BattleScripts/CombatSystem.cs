using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public enum SemiInvulnerable { None, Airborne, Underground, Underwater }
public enum TargetType {Self, Single, Foes, Ally, RandomFoe, All}
public class CombatSystem : MonoBehaviour
{
    private bool battleEndSignal;
    private int escapeAttempts;
    public static bool BattleActive {get; private set;}
    public static bool PlayerVictory { get; private set; }
    public static Weather Weather {get; set;}
    public static int weatherTimer;
    public static int TurnCount {get; private set;}
    public static BattleTarget ActiveTarget {get; private set;}
    public static MoveRecordList MoveRecordList {get; private set;}
    public CombatScreen combatScreen;
    public MoveFunctions moveFunctions;
    public HandleExperience handleExperience;
    public HandleEvolution handleEvolution;
    public GameObject struggle;
    public GameObject switchAction;
    public GameObject failedRunAction;
    public bool DoubleBattle {get; private set;}
    private Party playerParty;
    public Party EnemyParty { get; private set; }
    [SerializeField] private EnemyAI wildAI;
    private EnemyAI enemyAI;
    [SerializeField] private AudioClip runAwaySound;
    [SerializeField] private AudioPlayer wildMusic;
    [SerializeField] private AudioPlayer wildVictoryMusic;
    [SerializeField] private AudioPlayer areaMusic;
    [SerializeField] private OverlayTransitionCaller transitionCaller;
    private AudioPlayer battleMusicPlayer;
    private AudioPlayer victoryMusicPlayer;
    public static Trainer EnemyTrainer { get; private set; }
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
    //for trainer battles
    public IEnumerator StartBattle(Trainer trainer){
        EnemyTrainer = trainer;
        yield return StartCoroutine(RealStartBattle(new Party(trainer.trainerPartyTemplate), trainer.isDoubleBattle, trainer.trainerAI, trainer.battleMusic, trainer.introAnimation));
    }

    //for special wild encounters
    public IEnumerator StartBattle(Pokemon p, EnemyAI enemyAI, AudioPlayer encounterMusic, EventAnimation intro) {
        EnemyTrainer = null;
        yield return StartCoroutine(RealStartBattle(new Party(p), false, enemyAI, encounterMusic, intro));
    }

    //for normal wild battles
    public IEnumerator StartBattle(Pokemon p, EventAnimation intro){
        EnemyTrainer = null;
        yield return StartCoroutine(RealStartBattle(new Party(p), false, wildAI, wildMusic, intro));
    }

    //only used by battle test menu
    public void StartBattle(Party enemyParty, bool doubleBattle){
        EnemyTrainer = null;
        StartCoroutine(RealStartBattle(enemyParty, doubleBattle, wildAI, wildMusic));
    }

    private IEnumerator RealStartBattle(Party enemyParty, bool doubleBattle, EnemyAI enemyAI, AudioPlayer musicPlayer, EventAnimation introAnimation = null){
        BattleActive = true;
        TurnCount = 0;
        escapeAttempts = 0;
        Weather = ReferenceLib.Instance.activeArea.weather;
        weatherTimer = 0;
        MoveRecordList = new MoveRecordList();
        playerParty = PlayerParty.Instance.playerParty;
        this.DoubleBattle = doubleBattle;
        this.EnemyParty = enemyParty;
        this.enemyAI = enemyAI;
        this.battleMusicPlayer = musicPlayer;
        victoryMusicPlayer = EnemyTrainer != null ? EnemyTrainer.victoryMusic : wildVictoryMusic;

        musicPlayer.PlaySound();

        TeamBattleModifier playerTeamModifier = new TeamBattleModifier(EnemyTrainer != null, true);
        TeamBattleModifier enemyTeamModifier = new TeamBattleModifier(EnemyTrainer != null, false);

        player1 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(null), playerParty.GetFirstAvailable(), combatScreen.player1hud, combatScreen.player1Object);
        enemy1 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(null), this.EnemyParty.GetFirstAvailable(), combatScreen.enemy1hud, combatScreen.enemy1Object);

        player2 = new BattleTarget(playerTeamModifier, new IndividualBattleModifier(null), null, combatScreen.player2hud, combatScreen.player2Object);
        enemy2 = new BattleTarget(enemyTeamModifier, new IndividualBattleModifier(null), null, combatScreen.enemy2hud, combatScreen.enemy2Object);

        referenceBattleTargets = new List<BattleTarget>(){player1, enemy1};
        if(doubleBattle){
            player2.pokemon = playerParty.GetFirstAvailable();
            enemy2.pokemon = enemyParty.GetFirstAvailable();
            referenceBattleTargets.AddRange(new List<BattleTarget>(){player2, enemy2});
        }
        
        BattleTargets = new List<BattleTarget>(referenceBattleTargets.FindAll(b => b.pokemon != null));

        if(introAnimation != null){
            yield return StartCoroutine(introAnimation.TransitionLogic());
            combatScreen.barsOpening.previousTransitionToDestroy = introAnimation;
        }
        combatScreen.gameObject.SetActive(true);
        combatScreen.SetBattleSpriteFormat(doubleBattle);
        combatScreen.SetStartingGraphics(enemy1);

        //animation!
        yield return StartCoroutine(combatScreen.OpeningAnimationSequence(BattleTargets, enemy1));

        //make sure all mons active at the start of the battle are registered for experience
        handleExperience.UpdateParticipantsOnShift(BattleTargets);

        //check tag-in effects like intimidate, trace, etc.

        StartCoroutine(GetTurnActions());

        //waits until the battle is over before releasing control to the originating event chain
        yield return new WaitUntil(() => !BattleActive);
    }

    public static bool ActiveTargetCanSwitchOut(){
        return ActiveTarget.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyBind || e.effect is ApplyTrap || e.effect is ApplyIngrain) == null;      
    }

    public Party GetTeamParty(BattleTarget whoseTeam){
        return whoseTeam.teamBattleModifier.isPlayerTeam ? playerParty : EnemyParty;
    }

    public void OpenPartyMenu(){
        ActiveTarget.turnAction = switchAction;
        combatScreen.SetPartyScreen();
    }

    private IEnumerator GetTurnActions(){
        combatScreen.battleText.gameObject.SetActive(false);

        foreach(BattleTarget b in BattleTargets){
            if(battleEndSignal){
                yield return StartCoroutine(EndBattle());
                yield break;
            }

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

        List<GameObject> unusableMoves = MoveFunctions.GetAllUnusableMoves(ActiveTarget);

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

    public void RunButtonFunction(){
        StartCoroutine(AttemptToEscape());
    }

    private IEnumerator AttemptToEscape(){
        escapeAttempts++;
        if(EnemyTrainer == null){
            combatScreen.HideActionPanel();
            if(CheckRunAttempt(ActiveTarget.pokemon.stats[5], enemy1.pokemon.stats[5])){
                AudioManager.Instance.PlaySoundEffect(runAwaySound);
                yield return StartCoroutine(combatScreen.battleText.WriteMessageConfirm("Got away safely"));
                battleEndSignal = true;
            }
            else{
                ActiveTarget.turnAction = failedRunAction;
                yield return StartCoroutine(combatScreen.battleText.WriteMessageConfirm("Can't escape"));
            }
            Proceed = true;
            yield break;
        }
        yield return StartCoroutine(combatScreen.battleText.WriteMessageConfirm("There's no running from a trainer battle!"));
        combatScreen.battleText.gameObject.SetActive(false);
    }

    private bool CheckRunAttempt(int userSpeed, int enemySpeed){
        if(ActiveTarget.pokemon.stats[5] >= enemy1.pokemon.stats[5]){
            return true;
        }
        int escapeSeed = ((((userSpeed * 128) / enemySpeed) + 30) * escapeAttempts) % 256;
        return Random.Range(0, 256) < escapeSeed;
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

        user.individualBattleModifier.targets.RemoveAll(b => !BattleTargets.Contains(b) && b != null);
        TargetType moveTargetType = user.turnAction.GetComponent<MoveData>().targetType;
        if((moveTargetType == TargetType.Single || moveTargetType == TargetType.RandomFoe) && user.individualBattleModifier.targets.Count == 0){
            user.individualBattleModifier.targets = new List<BattleTarget>(){BattleTargets.First(b => b.teamBattleModifier.isPlayerTeam != user.teamBattleModifier.isPlayerTeam)};
        }
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
        
        MultiTurnEffect multi = move.GetComponent<MultiTurnEffect>();
        if(multi == null || !multi.chargingTurn){
            yield return StartCoroutine(combatScreen.battleText.WriteMessage(user.GetName() + " used " + moveData.moveName));
        }
    
        //must save value of target count or calling moves like mirror move may activate multiple times during double battles
        int targetCount = user.individualBattleModifier.targets.Count;
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

                    if(target.pokemon.CurrentHealth == 0){
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
            else{
                user.individualBattleModifier.multiTurnInfo = null;
            }

            if(!moveFailed.failed && user.individualBattleModifier.targets.Count > 0){
                MoveRecordList.AddRecord(user.pokemon, user.individualBattleModifier.targets[j].pokemon, user.turnAction);
            }

            if(MoveFunctions.IsChargingTurn(move)){
                break;
            }
        }

        yield return StartCoroutine(CheckOnFaintEffects(user, user.individualBattleModifier.targets));
    }

    private IEnumerator BattleTurn(){
        //increment turn count, reset damage taken this turn, destroy instantiated turnAction gameobjects, other cleanup things
        TurnCount++;
        foreach(BattleTarget b in BattleTargets){
            b.individualBattleModifier.specialDamageTakenThisTurn = 0;
            b.individualBattleModifier.physicalDamageTakenThisTurn = 0;
        }

        List<BattleTarget> turnOrder = new List<BattleTarget>(moveFunctions.GetTurnOrder(BattleTargets));

        for(int i = 0; i < turnOrder.Count; i++){
            if(battleEndSignal){
                break;
            }

            ActiveTarget = turnOrder[i];
            if(!BattleTargets.Contains(ActiveTarget)){
                continue;
            }

            GameObject action = Instantiate(ActiveTarget.turnAction);

            AppliedEffectInfo onFaintInfo = ActiveTarget.individualBattleModifier.appliedEffects.Find(effectInfo => effectInfo.effect is ApplyOnFaintEffect);
            if(onFaintInfo != null){
                onFaintInfo.effect.RemoveEffect(ActiveTarget, onFaintInfo);
            }

            if(action.CompareTag("Move")){
                BattleTarget swiper = SnatchOrMagicCoat(ActiveTarget, turnOrder);
                BattleTarget localUser = ActiveTarget;
                if(swiper != null){
                    localUser = swiper;
                    string typeOfSwipe = action.GetComponent<MoveData>().targetType == TargetType.Self ? " snatched " : " reflected ";
                    yield return StartCoroutine(combatScreen.battleText.WriteMessage(swiper.GetName() + typeOfSwipe + ActiveTarget.GetName() + "'s " + action.GetComponent<MoveData>().moveName + "!"));
                }

                PreMoveEffects(localUser, ActiveTarget.turnAction);
                yield return StartCoroutine(UseMove(localUser, action, false, true));
                PostMoveEffects(localUser, ActiveTarget.turnAction);
            }

            else if(action.CompareTag("Switch")){
                yield return StartCoroutine(action.GetComponent<MoveEffect>().DoEffect(ActiveTarget, null, null));
            }

            else if(action.CompareTag("Run")){
                continue;
            }

            yield return StartCoroutine(HandleFaint());

            if(IsAnyTeamEmpty()){
                break;
            }
        }

        if(playerParty.IsEntireTeamFainted() || EnemyParty.IsEntireTeamFainted() || battleEndSignal){
            yield return StartCoroutine(EndBattle());
            yield break;
        }

        turnOrder.RemoveAll(b => !BattleTargets.Contains(b));
        yield return StartCoroutine(moveFunctions.EndOfTurnEffects(turnOrder));

        if(playerParty.IsEntireTeamFainted() || EnemyParty.IsEntireTeamFainted()){
            yield return StartCoroutine(EndBattle());
            yield break;
        }

        yield return StartCoroutine(ReplaceFaintedTargets());

        StartCoroutine(GetTurnActions());
    }

    private IEnumerator CheckOnFaintEffects(BattleTarget user, List<BattleTarget> targets){
        for(int i = 0; i < targets.Count; i++){
            AppliedEffectInfo a = targets[i].individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyOnFaintEffect);
            if(a != null){
                IApplyEffect onFaintEffect = (IApplyEffect)a.effect;
                yield return StartCoroutine(onFaintEffect.DoAppliedEffect(user, a));
            }
        }
    }

    private BattleTarget SnatchOrMagicCoat(BattleTarget user, List<BattleTarget> turnOrder){
        MoveData moveData = user.turnAction.GetComponent<MoveData>();
        if(moveData.category == MoveData.Category.Status && moveData.targetType == TargetType.Self && !moveData.cannotBeSnatched){
            BattleTarget usedSnatch = turnOrder.Find(b => b.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplySnatch) != null);
            if(usedSnatch != null){
                usedSnatch.individualBattleModifier.appliedEffects.RemoveAll(e => e.effect is ApplySnatch);
                usedSnatch.turnAction = user.turnAction;
                return usedSnatch;
            }
        }
        BattleTarget targetThatUsedMagicCoat = user.individualBattleModifier.targets.Find(b => b.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyMagicCoat) != null);
        if(moveData.category == MoveData.Category.Status && targetThatUsedMagicCoat != null && !moveData.notReflectedByMagicCoat){
            targetThatUsedMagicCoat.turnAction = user.turnAction;
            if(moveData.targetType == TargetType.Single){
                targetThatUsedMagicCoat.individualBattleModifier.targets = new List<BattleTarget>(){user};
            }
            else{
                moveFunctions.MustChooseTarget(moveData.targetType, targetThatUsedMagicCoat);
            }
            return targetThatUsedMagicCoat;
        }
        return null;
    }

    private bool IsAnyTeamEmpty(){
        return BattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam) == null || BattleTargets.Find(b => !b.teamBattleModifier.isPlayerTeam) == null;
    }

    private void PreMoveEffects(BattleTarget user, GameObject moveUsed){
        if(moveUsed.GetComponent<ApplyCurse>() != null && user.pokemon.IsThisType(Pokemon.Type.Ghost)){
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

    public void ForceBattleEnd() {
        battleEndSignal = true;
    }

    public IEnumerator HandleFaint(){
        //IEnumerable.OrderBy sorts in ascending order, so boolean matches are listed in "reverse" order, hence the !
        List<BattleTarget> playerFirstTargets = new List<BattleTarget>(BattleTargets.OrderBy(b => !b.teamBattleModifier.isPlayerTeam).ToList());
        foreach(BattleTarget b in playerFirstTargets){
            if(b.pokemon.CurrentHealth == 0){
                b.pokemon.primaryStatus = PrimaryStatus.Fainted;
                b.individualBattleModifier = new IndividualBattleModifier(null);
                b.pokemon.inBattle = false;

                //magic numbers...
                AudioManager.Instance.PlaySoundEffect(b.pokemon.pokemonDefault.cry, 0.4f, -0.2f);
                yield return new WaitForSeconds(b.pokemon.pokemonDefault.cry.length * 1.4f);

                //animation for fainting, remove direct sprite object change
                b.monSpriteObject.SetActive(false);
                b.battleHUD.SlideOut();
                yield return StartCoroutine(combatScreen.battleText.WriteMessageConfirm(b.GetName() + " fainted"));
                //do xp here if fainted mon is opponent and trainer battle; xp for wild battles is handled in battle end logic
                if(!b.teamBattleModifier.isPlayerTeam && EnemyTrainer != null){
                    yield return StartCoroutine(handleExperience.DoExperience(b.pokemon));
                }
                else{
                    handleExperience.RemoveParticipant(b.pokemon);
                }
            }
        }

        BattleTargets.RemoveAll(b => b.pokemon.primaryStatus == PrimaryStatus.Fainted);
    }

    public IEnumerator ReplaceFaintedTargets(){
        List<BattleTarget> targetsToReplace = new List<BattleTarget>(referenceBattleTargets.FindAll(b => !BattleTargets.Contains(b)));
        foreach(BattleTarget needsReplaced in targetsToReplace){
            ActiveTarget = needsReplaced;
            ActiveTarget.individualBattleModifier.switchingIn = null; //may be necessary to prevent auto-switching to mon selected before being KO'ed by pursuit
            if(!ActiveTarget.teamBattleModifier.isPlayerTeam){
                ActiveTarget.individualBattleModifier.switchingIn = enemyAI.SelectNextPokemon(EnemyParty);
            }
            else{
                if(playerParty.HasAvailableFighter()){
                    combatScreen.SetPartyScreen(false, "Who will replace " + ActiveTarget.pokemon.nickName + "?");
                }
                else{
                    Proceed = true;
                }
                yield return new WaitUntil(() => Proceed);
            }
        }
        foreach(BattleTarget needsReplaced in targetsToReplace){
            if(needsReplaced.individualBattleModifier.switchingIn != null){
                yield return StartCoroutine(SendOutPokemon(needsReplaced, false));
                BattleTargets.Insert(referenceBattleTargets.IndexOf(needsReplaced), needsReplaced);
            }
        }
        handleExperience.UpdateParticipantsOnShift(BattleTargets);
    }

    public IEnumerator SwitchPokemon(BattleTarget replacing, bool passEffects){
        //withdraw animation goes in here somewhere
        if(replacing.individualBattleModifier.switchingIn == null){
            if(!replacing.teamBattleModifier.isPlayerTeam){
                replacing.individualBattleModifier.switchingIn = enemyAI.SelectNextPokemon(EnemyParty);
            }
            else{
                combatScreen.SetPartyScreen(false);
                yield return new WaitUntil(() => Proceed);
            }
        }
        yield return StartCoroutine(SendOutPokemon(replacing, passEffects));
    }

    public IEnumerator SendOutPokemon(BattleTarget replacing, bool passEffects){
        replacing.pokemon.inBattle = false;
        MoveRecordList.RemoveAllRecordsOfUser(replacing.pokemon);

        replacing.pokemon = replacing.individualBattleModifier.switchingIn;
        replacing.pokemon.inBattle = true;

        //account for baton pass
        replacing.individualBattleModifier = passEffects 
        ? new IndividualBattleModifier(replacing.individualBattleModifier, replacing.individualBattleModifier.timedEffects) 
        : new IndividualBattleModifier(replacing.individualBattleModifier.timedEffects);
        
        replacing.battleHUD.SetBattleHUD(replacing.pokemon);
        replacing.monSpriteObject.GetComponent<Image>().sprite = replacing.teamBattleModifier.isPlayerTeam ? replacing.pokemon.backSprite : replacing.pokemon.frontSprite;
        
        //replace setActives with animations
        replacing.monSpriteObject.SetActive(true);
        replacing.battleHUD.SlideIn();

        handleExperience.UpdateParticipantsOnShift(BattleTargets);

        string message = replacing.teamBattleModifier.isPlayerTeam ? "Go " + replacing.pokemon.nickName + "!" : "Enemy sent out " + replacing.pokemon.nickName;
        yield return StartCoroutine(combatScreen.battleText.WriteMessage(message));
    }

    public bool CanBeSwitchedIn(Pokemon pokemonToSwitchIn){
        return ActiveTargetCanSwitchOut() && pokemonToSwitchIn != null 
        && pokemonToSwitchIn.primaryStatus != PrimaryStatus.Fainted && !pokemonToSwitchIn.inBattle 
        && referenceBattleTargets.Find(b => b.individualBattleModifier.switchingIn == pokemonToSwitchIn) == null;
    }

    public IEnumerator HandleTeamEffects(){
        TeamBattleModifier playerTeam = referenceBattleTargets.Find(b => b.teamBattleModifier.isPlayerTeam).teamBattleModifier;
        TeamBattleModifier enemyTeam = referenceBattleTargets.Find(b => !b.teamBattleModifier.isPlayerTeam).teamBattleModifier;

        yield return StartCoroutine(OneTeamEffects(playerTeam));
        yield return StartCoroutine(OneTeamEffects(enemyTeam));
    }

    private IEnumerator OneTeamEffects(TeamBattleModifier whichTeam){
        foreach(TeamDurationEffectInfo t in whichTeam.teamEffects){
            t.timer--;
            if(t.timer == 0){
                yield return StartCoroutine(combatScreen.battleText.WriteMessage(t.effect.GetEndMessage(whichTeam)));
            }
        }
        whichTeam.teamEffects.RemoveAll(t => t.timer == 0);
    }

    private IEnumerator EndBattle(){
        //victory is overridden in the case of explicit loss; treat e.g. whirlwind exits as wins
        PlayerVictory = true;
        if(playerParty.IsEntireTeamFainted()){
            yield return StartCoroutine(combatScreen.battleText.WriteMessageConfirm("You lose, moron"));
            HandleEvolution.ClearMarkedLevelUps();
            PlayerVictory = false;
        }
        else if(EnemyParty.IsEntireTeamFainted()){
            victoryMusicPlayer.PlaySound();
            if(EnemyTrainer != null){
                yield return StartCoroutine(combatScreen.EndTrainerBattleSequence(EnemyTrainer));
            }
            else{
                //wild battle experience is handled after the battle is considered won
                yield return StartCoroutine(handleExperience.DoExperience(enemy1.pokemon));
            }
            yield return StartCoroutine(handleEvolution.DoEvolutions());
        }
        else{
            //battle was forcefully ended via e.g. run, whirlwind
        }
        //does this playSound ultimately belong here??
        areaMusic.PlaySound();

        yield return StartCoroutine(transitionCaller.CallTransitionCoroutine());

        battleEndSignal = false;
        BattleActive = false;
    }

    public void CleanUpAfterBattle(){
        foreach(Pokemon p in playerParty.party){
            if(p != null){
                p.inBattle = false;
            }
        }

        GameObject[] instantiatedMoves = GameObject.FindGameObjectsWithTag("Move");
        foreach(GameObject oldTurnAction in instantiatedMoves){
            Destroy(oldTurnAction);
        }

        GameObject[] instantiatedSwitchActions = GameObject.FindGameObjectsWithTag("Switch");
        foreach(GameObject oldTurnAction in instantiatedSwitchActions){
            Destroy(oldTurnAction);
        }
    }
}
