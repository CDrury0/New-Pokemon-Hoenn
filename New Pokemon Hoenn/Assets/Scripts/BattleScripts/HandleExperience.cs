using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleExperience : MonoBehaviour
{
    [SerializeField] private GameObject learnMoveScreenPrefab;
    [SerializeField] private GameObject levelUpScreenPrefab;
    private Dictionary<Pokemon, List<Pokemon>> expMaps;

    private void AddExpRecord(Pokemon enemy) {
        expMaps.TryAdd(enemy, new List<Pokemon>());
    }

    private void AddParticipant(Pokemon enemy, Pokemon participant) {
        if(!expMaps[enemy].Contains(participant)){
            expMaps[enemy].Add(participant);
        }
    }

    public void UpdateParticipantsOnShift(List<BattleTarget> battleTargets) {
        List<BattleTarget> enemies = battleTargets.FindAll(b => !b.teamBattleModifier.isPlayerTeam);
        List<BattleTarget> playerMons = battleTargets.FindAll(b => b.teamBattleModifier.isPlayerTeam);

        foreach(BattleTarget b in enemies){
            AddExpRecord(b.pokemon);
        }

        foreach(BattleTarget player in playerMons){
            foreach(BattleTarget enemy in enemies){
                AddParticipant(enemy.pokemon, player.pokemon);
            }
        }
    }

    public void RemoveParticipant(Pokemon participant) {
        foreach(List<Pokemon> list in expMaps.Values){
            list.Remove(participant);
        }
    }

    public IEnumerator DoBattleExperience(Pokemon enemy, System.Func<string, IEnumerator> messageOutput) {
        List<Pokemon> participants = expMaps[enemy];
        if(participants.Count == 0){
            yield break;
        }
        int totalExperience = enemy.pokemonDefault.baseExperience * enemy.level / 6;  //magic 6 means every 6 levels adds a multiple of base experience
        int experiencePerMon = totalExperience / participants.Count;
        foreach(Pokemon p in participants){
            yield return StartCoroutine(DoIndividualExperience(p, experiencePerMon, messageOutput));
        }
    }

    public IEnumerator DoIndividualExperience(Pokemon p, int amount, System.Func<string, IEnumerator> messageOutput) {
        //experience modifiers go here
        yield return StartCoroutine(messageOutput(p.nickName + " earned " + amount + " EXP"));
        BattleHUD inBattleHud = CombatSystem.GetBattleTarget(p)?.battleHUD;
        while(amount != 0){
            int expAtNextLevel = p.pokemonDefault.CalculateExperienceAtLevel(p.level + 1);
            //this iteration, grant either all experience or the experience required to reach the next level (whichever is smaller)
            int expToApply = Mathf.Min(amount, expAtNextLevel - p.experience);
            amount -= expToApply;  //exp will have ^this much less in it next iteration
            if(inBattleHud != null){
                yield return StartCoroutine(inBattleHud.expBar.SetExpBar(p.experience + expToApply, p.pokemonDefault.CalculateExperienceAtLevel(p.level), expAtNextLevel));
            }
            p.experience += expToApply;
            if(p.experience == expAtNextLevel){
                yield return StartCoroutine(LevelUp(p, inBattleHud, messageOutput));
            }
        }
    }

    ///<summary>A null value for hud indicates that the mon levelling up is not currently in battle</summary>
    public IEnumerator LevelUp(Pokemon p, BattleHUD hud, System.Func<string, IEnumerator> messageOutput) {
        HandleEvolution.MarkLevelUp(p);
        p.level++;
        int[] oldStats = new int[p.stats.Length];
        p.stats.CopyTo(oldStats, 0);
        p.UpdateStats();
        hud?.SetBattleHUD(p);
        yield return StartCoroutine(messageOutput(p.nickName + " grew to level " + p.level + "!"));
        LevelUpScreen levelUp = Instantiate(levelUpScreenPrefab).GetComponent<LevelUpScreen>();
        yield return StartCoroutine(levelUp.DoLevelUpScreen(oldStats, p.stats, p.nickName));
        Destroy(levelUp.gameObject);
        GameObject learnedMove = LearnMoveScreen.GetValidMoveToLearn(p);
        if (learnedMove != null){
            LearnMoveScreen learnScreen = Instantiate(learnMoveScreenPrefab).GetComponent<LearnMoveScreen>();
            yield return StartCoroutine(learnScreen.DoLearnMoveScreen(p, learnedMove, (string message) => messageOutput(message)));
            Destroy(learnScreen.gameObject);
        }
    }

    void Awake() {
        expMaps = new Dictionary<Pokemon, List<Pokemon>>();
    }
}
