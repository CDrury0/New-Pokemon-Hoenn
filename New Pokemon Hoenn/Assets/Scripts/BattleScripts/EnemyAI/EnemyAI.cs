using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    private Trainer trainer;
    protected abstract void SetTurnAction(BattleTarget user);
    public abstract Pokemon SelectNextPokemon(Party enemyParty);

    protected List<GameObject> GetPossibleMoves(BattleTarget user){
        List<GameObject> unusableMoves = MoveFunctions.GetAllUnusableMoves(user);
        List<GameObject> usableMoves = user.pokemon.moves.FindAll(move => !unusableMoves.Contains(move));
        return usableMoves.Count == 0 ? new List<GameObject>(){CombatLib.Instance.combatSystem.struggle} : usableMoves;
    }

    /// <summary>
    /// creates a list of itemLogicGO prefabs that does not contain duplicates
    /// </summary>
    protected List<GameObject> GetUsableItems(Pokemon p){
        if(trainer == null){
            return new List<GameObject>();
        }
        HashSet<GameObject> itemSet = new HashSet<GameObject>();
        List<ItemData> trainerInventory = GetComponent<Trainer>().battleInventory;
        foreach(ItemData i in trainerInventory){
            if(i.itemLogicGO.GetComponent<ItemLogic>().CanItemBeUsedOn(p)){
                itemSet.Add(i.itemLogicGO);
            }
        }
        return itemSet.ToList();
    }

    public void ChooseAction(BattleTarget user){
        if(CombatLib.Instance.moveFunctions.LockedIntoAction(user)){
            return;
        }
        SetTurnAction(user);
        if(user.turnAction.CompareTag("Item")){
            trainer?.battleInventory.Remove(user.turnAction.GetComponent<ItemLogic>()?.itemData);
        }
    }

    void Awake(){
        trainer = GetComponent<Trainer>();
    }
}
