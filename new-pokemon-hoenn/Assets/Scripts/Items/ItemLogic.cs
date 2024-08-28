using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public bool usedOnSpecificMove;
    public bool usedWithoutTarget;
    public ItemData itemData;
    [Tooltip("If null, the item cannot be held")]
    public HeldItem heldItem;
    [Tooltip("If length is 0, the item cannot be used during battle")]
    [SerializeField] private List<ItemEffect> onUseDuringBattle;
    [Tooltip("If length is 0, the item cannot be used outside battle")]
    [SerializeField] private List<ItemEffect> onUseOutsideBattle;

    public bool GetAllowUse(){
        bool hasEffectsToUse = (CombatSystem.BattleActive && onUseDuringBattle.Count > 0) || (!CombatSystem.BattleActive && onUseOutsideBattle.Count > 0);
        if(usedWithoutTarget){
            return hasEffectsToUse && CanItemBeUsedOn(null);
        }
        return hasEffectsToUse;
    }

    public bool CanItemBeUsedOn(Pokemon p){
        IEnumerable<ItemEffect> itemEffects = CombatSystem.BattleActive ? onUseDuringBattle : onUseOutsideBattle;
        foreach(ItemEffect i in itemEffects){
            //if ANY effect cannot be used, the item cannot be used
            if(!i.CanEffectBeUsed(p)){
                return false;
            }
        }
        return true;
    }

    public IEnumerator DoItemEffects(BattleHUD hudObj, Pokemon p, System.Func<string, IEnumerator> messageOutput, int whichMove = -1){
        List<ItemEffect> effectList = CombatSystem.BattleActive ? onUseDuringBattle : onUseOutsideBattle;
        yield return StartCoroutine(DoEffectList(effectList, p, hudObj, messageOutput, whichMove));
        if(CombatSystem.BattleActive && p.inBattle){
            CombatSystem.GetBattleTarget(p).battleHUD.SetBattleHUD(p);
        }
    }

    private IEnumerator DoEffectList(List<ItemEffect> effects, Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        for (int i = 0; i < effects.Count - 1; i++){
            yield return StartCoroutine(effects[i].DoItemEffect(p, hudObj, null, whichMove));
        }
        //only the last effect in the list should have its message displayed
        yield return StartCoroutine(effects[^1].DoItemEffect(p, hudObj, messageOutput, whichMove));
    }
}
