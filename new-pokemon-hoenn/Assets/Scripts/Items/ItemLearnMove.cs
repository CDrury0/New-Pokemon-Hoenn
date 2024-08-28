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
        return canLearnMove.Contains(p.pokemonDefault)
        && !p.moves.Contains(moveToLearn)
        && p.moves.Find(m => m != null && !m.GetComponent<MoveData>().cannotBeForgotten) != null;
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
