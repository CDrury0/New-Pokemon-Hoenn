using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public abstract class ItemEffect : MonoBehaviour
{
    protected ItemLogic itemLogic;
    protected string message;
    protected abstract IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj);
    public abstract bool CanEffectBeUsed(Pokemon p);
    
    public IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput){
        yield return StartCoroutine(ItemEffectImplementation(p, hudObj));
        hudObj.SetBattleHUD(p);
        //if it succeeds, then the pokemon was in the player's party
        //if it doesn't, i don't give a shit
        try{
            hudObj.GetComponentInParent<PartyInfoBox>().LoadPokemonDetails(false);
        } catch(System.Exception e){
            Debug.LogError(e);
        }
        if(messageOutput != null){
            yield return StartCoroutine(messageOutput(message));
        }
    }

    void Awake(){
        itemLogic = GetComponent<ItemLogic>();
    }
}
