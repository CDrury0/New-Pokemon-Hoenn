using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public ItemData itemData;
    [Tooltip("If null, the item cannot be held")]
    public HeldItem heldItem;
    [Tooltip("If length is 0, the item cannot be used during battle")]
    public List<ItemEffect> onUseDuringBattle;
    [Tooltip("If length is 0, the item cannot be used outside battle")]
    public List<ItemEffect> onUseOutsideBattle;
    //public PokemonDefault[] worksOn;

    public IEnumerator DoItemEffects(BattleHUD hudObj, Pokemon p, System.Func<string, IEnumerator> messageOutput){
        PlayerInventory.SubtractItem(itemData);
        List<ItemEffect> effectList = CombatSystem.BattleActive ? onUseDuringBattle : onUseOutsideBattle;
        yield return StartCoroutine(DoEffectList(effectList, p, hudObj, messageOutput));
        if(CombatSystem.BattleActive && p.inBattle){
            CombatSystem.BattleTargets.Find(b => b.pokemon == p).battleHUD.SetBattleHUD(p);
        }
    }

    private IEnumerator DoEffectList(List<ItemEffect> effects, Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput){
        for (int i = 0; i < effects.Count - 1; i++){
            yield return StartCoroutine(effects[i].DoItemEffect(p, hudObj, null));
        }
        //only the last effect in the list should have its message displayed
        yield return StartCoroutine(effects[effects.Count - 1].DoItemEffect(p, hudObj, messageOutput));
    }
}
