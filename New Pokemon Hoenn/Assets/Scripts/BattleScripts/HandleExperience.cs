using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleExperience : MonoBehaviour
{
    [SerializeField] private LearnMoveScreen learnMoveScreen;
    [SerializeField] private LevelUpScreen levelUpScreen;
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

    public IEnumerator DoExperience(Pokemon enemy) {
        List<Pokemon> participants = expMaps[enemy];
        if(participants.Count == 0){
            yield break;
        }
        int totalExperience = enemy.pokemonDefault.baseExperience * enemy.level / 6;  //magic 6 means every 6 levels adds a multiple of base experience
        int experiencePerMon = totalExperience / participants.Count;
        foreach(Pokemon p in participants){
            int exp = experiencePerMon;
            //experience modifiers go here
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(p.nickName + " earned " + exp + " EXP"));
            BattleHUD hud = CombatSystem.BattleTargets.Find(b => b.pokemon == p)?.battleHUD;
            while(exp != 0){
                int expAtNextLevel = p.pokemonDefault.CalculateExperienceAtLevel(p.level + 1);
                int expToApply = Mathf.Min(exp, expAtNextLevel - p.experience); //this iteration, grant either all experience or the experience required to reach the next level (whichever is smaller)
                exp -= expToApply;  //exp will have ^this much less in it next iteration
                if(hud != null){
                    yield return StartCoroutine(hud.expBar.SetExpBar(p.experience + expToApply, p.pokemonDefault.CalculateExperienceAtLevel(p.level), expAtNextLevel));
                }
                p.experience += expToApply;
                if(p.experience == expAtNextLevel){
                    yield return StartCoroutine(LevelUp(p, hud));
                }
            }
        }
    }

    ///<summary>A null value for hud indicates that the mon levelling up is not currently in battle</summary>
    public IEnumerator LevelUp(Pokemon p, BattleHUD hud) {
        p.level++;
        int[] oldStats = new int[p.stats.Length];
        p.stats.CopyTo(oldStats, 0);
        p.UpdateStats();
        if(hud != null){
            hud.SetBattleHUD(p);
        }
        yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(p.nickName + " grew to level " + p.level + "!"));
        yield return StartCoroutine(levelUpScreen.DoLevelUpScreen(oldStats, p.stats, p.nickName));
        GameObject learnedMove = p.pokemonDefault.learnedMoves[p.level];
        if (learnedMove != null && !p.moves.Contains(learnedMove)){
            string moveName = learnedMove.GetComponent<MoveData>().moveName;
            yield return StartCoroutine(learnMoveScreen.DoLearnMoveScreen(p, learnedMove));
            yield return StartCoroutine(CombatLib.Instance.WriteGlobalMessage(
                p.nickName + (learnMoveScreen.MoveReplaced < p.moves.Count ? " learned " + moveName + "!" : " did not learn " + moveName)));
        }
    }

    void Awake() {
        expMaps = new Dictionary<Pokemon, List<Pokemon>>();
    }
}
