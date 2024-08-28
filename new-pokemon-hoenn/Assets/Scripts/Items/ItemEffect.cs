using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public abstract class ItemEffect : MonoBehaviour
{
    protected ItemLogic itemLogic;
    protected string message;

    /// <param name="hudObj">hudObj has SetBattleHUD(p) called on it after this method is executed; only use hudObj for other one-off HUD actions</param>
    /// <param name="messageOutput">messageOutput should be used only when required by another component; ordinary messages should be set in ItemEffect.message</param>
    /// <param name="whichMove">-1 by default, 0-3 represent the 4 moves the pokemon may know</param>
    protected abstract IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove);
    public abstract bool CanEffectBeUsed(Pokemon p);
    
    public IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        yield return StartCoroutine(ItemEffectImplementation(p, hudObj, messageOutput, whichMove));
        hudObj?.SetBattleHUD(p);
        //if it succeeds, then the pokemon was in the player's party
        //if it doesn't, i don't give a shit
        hudObj?.GetComponentInParent<PartyInfoBox>()?.LoadPokemonDetails(false);
        if (string.IsNullOrEmpty(message) || messageOutput == null){
            yield break;
        }
        yield return StartCoroutine(messageOutput(message));
    }

    void Awake(){
        itemLogic = GetComponent<ItemLogic>();
    }
}
