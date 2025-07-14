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
        var party = isPlayerParty ? PlayerParty.Party.members : CombatLib.CombatSystem.EnemyParty.members;
        List<int> indicesOfAvailableFighters = new();
        for(int i = 0; i < party.Count; i++){
            if(party[i].IsAvailableFighter())
                indicesOfAvailableFighters.Add(i);
        }
        if(indicesOfAvailableFighters.Count > 0)
            return party[indicesOfAvailableFighters[Random.Range(0, indicesOfAvailableFighters.Count)]];
            
        return null;
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
