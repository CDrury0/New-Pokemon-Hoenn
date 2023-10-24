using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : ItemEffect
{
    [SerializeField] private int flatHealAmount;
    [SerializeField] [Range(0, 1)] private float percentHpHealAmount;

    public override bool CanEffectBeUsed(Pokemon p){
        return p.CurrentHealth < p.stats[0];
    }

    public override IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput){
        int healAmount = Mathf.Min((int)(percentHpHealAmount * p.stats[0]) + flatHealAmount, p.stats[0] - p.CurrentHealth);
        yield return StartCoroutine(hudObj.healthBar.SetHealthBar(p, healAmount));
        p.CurrentHealth += healAmount;
        if(messageOutput != null){
            string message = p.nickName + "'s HP was restored by " + healAmount + " points";
            yield return StartCoroutine(messageOutput(message));
        }
    }

    private IEnumerator HealTarget(Pokemon p, BattleHUD hud){
        int totalHealAmount = flatHealAmount + (int)(percentHpHealAmount * p.stats[0]);
        yield return StartCoroutine(hud.healthBar.SetHealthBar(p, totalHealAmount));
        p.CurrentHealth += totalHealAmount;
    }
}
