using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image expBar;
    [SerializeField] private AudioClip barFillSound;
    [SerializeField] private AudioClip ding;
    [SerializeField] private SingleAnimOverride levelUpAnim;
    private AudioSource source;

    public IEnumerator SetExpBar(int targetExp, int minExp, int maxExp){
        source = AudioManager.Instance.PlaySoundEffect(barFillSound, 0f, -0.15f);
        int amountOfExpShown = maxExp - minExp;
        for(int i = 0; i < 200; i++){
            if(expBar.fillAmount >= (float)(targetExp - minExp) / (float)amountOfExpShown){
                expBar.fillAmount = (float)(targetExp - minExp) / (float)amountOfExpShown;
                break;
            }
            expBar.fillAmount += 0.005f;
            yield return new WaitForSeconds(0.01f);
        }
        if(source != null){
            source.Stop();
        }
        if(targetExp == maxExp){
            AudioManager.Instance.PlaySoundEffect(ding, -0.1f);
            yield return StartCoroutine(levelUpAnim.PlayAnimationWait());
            yield return new WaitForSeconds(0.25f);
        }
    }

    //only used to update battle hud when mon is sent out
    public void SetExpBarInstant(int targetExp, int minExp, int maxExp){
        expBar.fillAmount = (float)(targetExp - minExp) / (float)(maxExp - minExp);
    }
}
