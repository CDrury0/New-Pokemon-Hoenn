using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI //: MonoBehaviour enables use as component on trainer game objects (temporarily disabled to allow instantiation via script)
{
    protected List<GameObject> GetPossibleActions(BattleTarget user){
        List<GameObject> unusableMoves = CombatLib.Instance.combatSystem.GetAllUnusableMoves(user);
        List<GameObject> usableMoves = user.pokemon.moves.FindAll(move => !unusableMoves.Contains(move));
        return usableMoves.Count == 0 ? new List<GameObject>(){CombatLib.Instance.combatSystem.struggle} : usableMoves;
    }

    public abstract void ChooseAction(BattleTarget user);

    public abstract Pokemon SelectNextPokemon(Party enemyParty);
}
