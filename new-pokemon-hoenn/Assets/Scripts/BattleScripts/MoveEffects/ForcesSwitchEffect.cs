using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcesSwitchEffect : MoveEffect, ICheckMoveFail
{
    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData) {
        if(CombatSystem.EnemyTrainer != null){
            //set turn action to null, remove from list of users who will move this turn?
            target.individualBattleModifier.switchingIn = GetRandomAvailablePartyMember(target.teamBattleModifier.isPlayerTeam);
            yield return StartCoroutine(CombatLib.Instance.combatSystem.SendOutPokemon(target, false));
        }
        else{
            CombatLib.Instance.combatSystem.ForceBattleEnd();
        }
    }

    private Pokemon GetRandomAvailablePartyMember(bool isPlayerParty){
        List<Pokemon> party = new List<Pokemon>(isPlayerParty ? PlayerParty.Instance.playerParty.party : CombatLib.Instance.combatSystem.EnemyParty.party);
        List<int> indicesOfAvailableFighters = new List<int>();
        for (int i = 0; i < party.Count; i++){
            if(Party.CheckIsAvailableFighter(party[i])){
                indicesOfAvailableFighters.Add(i);
            }
        }
        return indicesOfAvailableFighters.Count > 0 ? party[indicesOfAvailableFighters[Random.Range(0, indicesOfAvailableFighters.Count)]] : null;
    }

    //suction cups/ingrain?
    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData) {
        if(target.individualBattleModifier.GetEffectInfoOfType<ApplyIngrain>() != null){
            return MoveData.FAIL;
        }
        if(user.pokemon.level < target.pokemon.level){
            return MoveData.FAIL;
        }
        if(CombatSystem.EnemyTrainer != null && GetRandomAvailablePartyMember(target.teamBattleModifier.isPlayerTeam) == null){
            return MoveData.FAIL;
        }
        return null;
    }
}
