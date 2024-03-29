using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private ColorSO colorList;

    public IEnumerator SetHealthBar(Pokemon p, int healthChange){
        int startingHealth = p.CurrentHealth;
        int targetHealth = startingHealth + healthChange;
        int maxHealth = p.stats[0];
        if(targetHealth > maxHealth){
            targetHealth = maxHealth;
        }
        else if(targetHealth < 0){
            targetHealth = 0;
        } 
        int healthDif = startingHealth - targetHealth;
        if(healthDif != 0){
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
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetHealthBarInstant(int targetHealth, int maxHealth){
        healthText.text = targetHealth + " / " + maxHealth;
        healthBar.fillAmount = (float)targetHealth / (float)maxHealth;
        SetColor();
    }

    private void SetColor(){
        if(healthBar.fillAmount >= 0.5f){
            healthBar.color = colorList.colors[2];
        }
        else if(healthBar.fillAmount >= 0.15f){
            healthBar.color = colorList.colors[1];
        }
        else{
            healthBar.color = colorList.colors[0];
        }
    }
}
