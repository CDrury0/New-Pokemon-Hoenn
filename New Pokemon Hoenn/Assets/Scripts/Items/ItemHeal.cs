using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : ItemEffect
{
    [SerializeField] private int flatHealAmount;
    [SerializeField] [Range(0, 1)] private float percentHpHealAmount;

    public override bool CanEffectBeUsed(Pokemon p){
        //cannot be used if target has fainted, unless this item also revives
        return p.CurrentHealth < p.stats[0] && (p.primaryStatus != PrimaryStatus.Fainted || ItemRevives());
    }

    private bool ItemRevives(){
        ItemCurePrimaryStatus cure = GetComponent<ItemCurePrimaryStatus>();
        return cure != null && cure.CureStatus == PrimaryStatus.Fainted;
    }

    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj){
        int healAmount = Mathf.Min((int)(percentHpHealAmount * p.stats[0]) + flatHealAmount, p.stats[0] - p.CurrentHealth);
        yield return StartCoroutine(hudObj.healthBar.SetHealthBar(p, healAmount));
        p.CurrentHealth += healAmount;
        message = p.nickName + "'s HP was restored by " + healAmount + " points";
    }

    private IEnumerator HealTarget(Pokemon p, BattleHUD hud){
        int totalHealAmount = flatHealAmount + (int)(percentHpHealAmount * p.stats[0]);
        yield return StartCoroutine(hud.healthBar.SetHealthBar(p, totalHealAmount));
        p.CurrentHealth += totalHealAmount;
    }
}
