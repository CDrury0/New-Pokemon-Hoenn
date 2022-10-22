using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image healthBar;
    public Color redHealth;
    public Color yellowHealth;
    public Color greenHealth;

    public IEnumerator SetHealthBar(int startingHealth, int targetHealth, int maxHealth){
        int healthDif = startingHealth - targetHealth;
        float healthRatio = maxHealth < 100 ? 100f / (float)maxHealth : 1f;
        int numSteps = (int)(Mathf.Abs(healthDif) * healthRatio);
        int direction = healthDif / Mathf.Abs(healthDif);
        float oneHealthWorthOfFill = 1f / (float)maxHealth;
        float howMuchOfOneHealthWorthOfFillPerStep = 1 / healthRatio;

        for(int i = 0; i < numSteps; i++){
            healthText.text = (int)(healthBar.fillAmount * maxHealth) + " / " + maxHealth;
            healthBar.fillAmount -= direction * howMuchOfOneHealthWorthOfFillPerStep * oneHealthWorthOfFill;
            SetColor();
            yield return new WaitForSeconds(0.01f);
        }
        SetHealthBarInstant(targetHealth, maxHealth);
    }

    public void SetHealthBarInstant(int targetHealth, int maxHealth){
        healthText.text = targetHealth + " / " + maxHealth;
        healthBar.fillAmount = (float)targetHealth / (float)maxHealth;
        SetColor();
    }

    private void SetColor(){
        if(healthBar.fillAmount >= 0.5f){
            healthBar.color = greenHealth;
        }
        else if(healthBar.fillAmount >= 0.15f){
            healthBar.color = yellowHealth;
        }
        else{
            healthBar.color = redHealth;
        }
    }
}
