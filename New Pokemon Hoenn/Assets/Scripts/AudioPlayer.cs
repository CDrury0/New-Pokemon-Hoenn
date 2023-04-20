using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : InteractEvent
{
    [SerializeField] private string label;
    public AudioManager.Sound soundToPlay;
    public AudioManager.Sound musicIntro;
    public AudioManager.Sound musicLoop;

    public override IEnumerator DoInteractEvent() {
        PlaySound();
        yield break;
    }

    public void PlaySound() {
        if(soundToPlay.clip != null){
            AudioManager.Instance.PlaySoundEffect(soundToPlay);
        }
        else{
            AudioManager.Instance.PlayMusic(musicIntro, musicLoop);
        }
    }
}
