using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLearnMove : ItemEffect
{
    [SerializeField] private GameObject learnMoveMenuPrefab;
    [SerializeField] private GameObject moveToLearn;
    [SerializeField] private bool reusable;
    [SerializeField] private List<PokemonDefault> canLearnMove;
    private LearnMoveScreen learnMoveScreen;

    public override bool CanEffectBeUsed(Pokemon p) {
        if(!canLearnMove.Contains(p.pokemonDefault))
            return false;

        if(p.moves.Contains(moveToLearn))
            return false;

        foreach(var move in p.moves){
            if(move is null)
                return true;

            var fieldMove = move.GetComponent<FieldMove>();
            if(fieldMove is null || !fieldMove.CannotBeForgotten)
                return true;
        }
        return false;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove) {
        GameObject learnScreenParent = Instantiate(learnMoveMenuPrefab);
        learnMoveScreen = learnScreenParent.GetComponentInChildren<LearnMoveScreen>();
        yield return StartCoroutine(learnMoveScreen.DoLearnMoveScreen(p, moveToLearn, messageOutput));
        //refund the item if it went unused or is reusable
        if(learnMoveScreen.MoveReplaced >= p.moves.Count || reusable){
            PlayerInventory.AddItem(itemLogic.itemData);
        }
        Destroy(learnScreenParent);
    }
}
