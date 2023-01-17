using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioManager.Sound soundToPlay;
    public AudioManager.Sound musicIntro;
    public AudioManager.Sound musicLoop;

    public void PlaySound(){
        if(soundToPlay.clip != null){
            AudioManager.Instance.PlaySoundEffect(soundToPlay);
        }
    }
}
