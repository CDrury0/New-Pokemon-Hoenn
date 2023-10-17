using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : ItemEffect
{
    [SerializeField] private int flatHealAmount;
    [SerializeField] [Range(0, 1)] private float percentHpHealAmount;

    public override bool CanEffectBeUsed(Pokemon p)
    {
        return p.CurrentHealth < p.stats[0];
    }

    public override IEnumerator DoItemEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string GetItemEffectMessage()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator HealTarget(Pokemon p, BattleHUD hud){
        int totalHealAmount = flatHealAmount + (int)(percentHpHealAmount * p.stats[0]);
        yield return StartCoroutine(hud.healthBar.SetHealthBar(p, totalHealAmount));
        p.CurrentHealth += totalHealAmount;
    }
}
