using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private string label;
    [Tooltip("If music is taken from current GameArea, clip references are not necessary")]
    public bool getClipsFromArea;
    public bool fadeOutMusic;
    [Tooltip("Percentage by which to reduce the music volume while this sound effect is playing")]
    public float musicVolumeReduction;
    public AudioClip soundToPlay;
    public AudioClip musicIntro;
    public AudioClip musicLoop;

    public void PlaySound() {
        if(soundToPlay != null){
            AudioManager.Instance.PlaySoundEffect(soundToPlay, musicVolumeReduction);
            return;
        }
        else if(getClipsFromArea){
            AreaData area = ReferenceLib.ActiveArea;
            musicIntro = area.musicIntro;
            musicLoop = area.musicLoop;
        }

        if(!(musicIntro == null && musicLoop == null)){
            AudioManager.Instance.PlayMusic(musicIntro, musicLoop, fadeOutMusic);
        }
    }
}
