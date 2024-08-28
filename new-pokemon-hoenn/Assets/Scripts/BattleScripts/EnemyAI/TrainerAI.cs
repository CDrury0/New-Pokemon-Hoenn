using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainerAI : EnemyAI
{
    [SerializeField] private Trainer trainer;

    public override Pokemon SelectNextPokemon(Party enemyParty){
        return enemyParty.GetFirstAvailable();
    }

    protected override void SetTurnAction(BattleTarget user){
        List<GameObject> possibleActions = GetPossibleMoves(user);
        possibleActions.AddRange(GetUsableItems(user.pokemon));
        user.turnAction = possibleActions[Random.Range(0, possibleActions.Count)];
        if(CombatLib.Instance.moveFunctions.MustChooseTarget(user.turnAction.GetComponent<MoveData>().targetType, user)){
            CombatLib.Instance.moveFunctions.MustChooseTarget(TargetType.RandomFoe, user);
        }
        if(user.turnAction.CompareTag("Item")){
            trainer.battleInventory.Remove(user.turnAction.GetComponent<ItemLogic>().itemData);
        }
    }

    /// <summary>
    /// creates a list of itemLogicGO prefabs that does not contain duplicates
    /// </summary>
    protected List<GameObject> GetUsableItems(Pokemon p){
        HashSet<GameObject> itemSet = new HashSet<GameObject>();
        List<ItemData> trainerInventory = GetComponent<Trainer>().battleInventory;
        foreach(ItemData i in trainerInventory){
            if(i.itemLogicGO.GetComponent<ItemLogic>().CanItemBeUsedOn(p)){
                itemSet.Add(i.itemLogicGO);
            }
        }
        return itemSet.ToList();
    }
}
