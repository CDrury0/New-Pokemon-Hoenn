using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public Color hpGreen;
    public Color hpYellow;
    public Color hpRed;
    public bool proceedHealth;
    private int healthDifference;

    public void SetHealthBarFixed(Image hpFill, Text hpText, int currentHealth, int maxHealth, int referenceHealth)
    {
        hpFill.fillAmount = (float)(currentHealth) / (float)(maxHealth);
        hpText.text = currentHealth + "/" + maxHealth;
        SetBarColor(hpFill);
    }

    public IEnumerator ChangeFill(Image hpFill, Text hpText, int currentHealth, int maxHealth, int referenceHealth)
    {
        bool losingHealth = currentHealth < referenceHealth;
        healthDifference = losingHealth ? referenceHealth - currentHealth : currentHealth - referenceHealth;

        for (int i = 0; i < healthDifference; i++)
        {
            int displayHealth = referenceHealth + (losingHealth ? -(i+1) : (i+1));
            hpText.text = displayHealth + "/" + maxHealth;
            float tempFill = hpFill.fillAmount;
            hpFill.fillAmount += losingHealth ? -(tempFill - Mathf.Abs((float)displayHealth / (float)maxHealth)) : (Mathf.Abs((float)displayHealth / (float)maxHealth) - tempFill);
            SetBarColor(hpFill);
            yield return new WaitForSeconds(0.02f);
        }
        SetHealthBarFixed(hpFill, hpText, currentHealth, maxHealth, referenceHealth);
        yield return new WaitForSeconds(0.5f);
        proceedHealth = true;
    }

    private void SetBarColor(Image hpFill)
    {
        if (hpFill.fillAmount < 0.2f)
        {
            hpFill.color = hpRed;
        }
        else if (hpFill.fillAmount < 0.5f)
        {
            hpFill.color = hpYellow;
        }
        else
        {
            hpFill.color = hpGreen;
        }
    }
}
