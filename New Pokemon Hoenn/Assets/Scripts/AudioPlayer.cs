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
    public AudioClip soundToPlay;
    public AudioClip musicIntro;
    public AudioClip musicLoop;

    protected override IEnumerator EventActionLogic() {
        PlaySound();
        yield break;
    }

    public void PlaySound() {
        if(soundToPlay != null){
            AudioManager.Instance.PlaySoundEffect(soundToPlay, reduceMusic);
        }
        else{
            if(getClipsFromArea){
                AreaData area = ReferenceLib.Instance.activeArea;
                musicIntro = area.musicIntro;
                musicLoop = area.musicLoop;
            }
            AudioManager.Instance.PlayMusic(musicIntro, musicLoop, fadeOutMusic);
        }
    }
}
