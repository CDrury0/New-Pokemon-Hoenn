using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : EventAction
{
    [SerializeField] private string label;
    [Tooltip("If music is taken from current GameArea, clip references are not necessary")]
    public bool getClipsFromArea;
    public bool fadeOutMusic;
    public bool reduceMusic;
    public AudioManager.Sound soundToPlay;
    public AudioManager.Sound musicIntro;
    public AudioManager.Sound musicLoop;

    protected override IEnumerator EventActionLogic() {
        PlaySound();
        yield break;
    }

    public void PlaySound() {
        if(soundToPlay.clip != null){
            AudioManager.Instance.PlaySoundEffect(soundToPlay, reduceMusic);
        }
        else{
            if(getClipsFromArea){
                AreaData area = ReferenceLib.Instance.activeArea;
                musicIntro.clip = area.musicIntro;
                musicLoop.clip = area.musicLoop;
            }
            AudioManager.Instance.PlayMusic(musicIntro, musicLoop, fadeOutMusic);
        }
    }
}
