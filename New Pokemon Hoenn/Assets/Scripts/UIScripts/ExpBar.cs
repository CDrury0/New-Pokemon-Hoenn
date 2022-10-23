using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image expBar;

    public IEnumerator SetExpBar(int targetExp, int minExp, int maxExp){
        int amountOfExpShown = maxExp - minExp;
        for(int i = 0; i < 50; i++){
            if(expBar.fillAmount >= (float)(targetExp - minExp) / (float)amountOfExpShown){
                expBar.fillAmount = (float)(targetExp - minExp) / (float)amountOfExpShown;
                break;
            }
            expBar.fillAmount += 0.02f;
            yield return new WaitForSeconds(0.03f);
        }
        //if targetExp == maxExp, do level up bar animation
    }

    public void SetExpBarInstant(int targetExp, int minExp, int maxExp){
        expBar.fillAmount = (float)(targetExp - minExp) / (float)(maxExp - minExp);
    }
}
