using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    protected abstract void SetTurnAction(BattleTarget user);
    public abstract Pokemon SelectNextPokemon(Party enemyParty);

    protected List<GameObject> GetPossibleMoves(BattleTarget user){
        List<GameObject> unusableMoves = MoveFunctions.GetAllUnusableMoves(user);
        List<GameObject> usableMoves = user.pokemon.moves.FindAll(move => !unusableMoves.Contains(move));
        return usableMoves.Count == 0 ? new List<GameObject>(){CombatLib.Instance.combatSystem.struggle} : usableMoves;
    }

    public void ChooseAction(BattleTarget user){
        if(CombatLib.Instance.moveFunctions.LockedIntoAction(user)){
            return;
        }
        SetTurnAction(user);
    }
}
