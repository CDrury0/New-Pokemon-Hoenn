using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource musicSource2;
    [SerializeField] private AudioSource soundEffectSource;
    private IEnumerator currentMusicCycle;

    [System.Serializable]
    public class Sound{
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public void PlaySoundEffect(Sound sound){
        soundEffectSource.volume = sound.volume;
        soundEffectSource.PlayOneShot(sound.clip);
    }

    public void PlayMusic(Sound intro, Sound loop){
        if(musicSource.isPlaying){
            musicSource.Stop();
            if(currentMusicCycle != null){
                StopCoroutine(currentMusicCycle);
            }
        }
        currentMusicCycle = DoMusicCycle(intro, loop);
        StartCoroutine(currentMusicCycle);
    }

    private IEnumerator DoMusicCycle(Sound intro, Sound loop){
        musicSource.clip = intro.clip;
        musicSource2.clip = loop.clip;
        musicSource.Play();
        yield return new WaitUntil(() => !musicSource.isPlaying);
        musicSource2.Play();
    }

    void Awake(){
        if(Instance != null){
            Debug.Log("AudioManager instance exists");
            return;
        }
        Instance = this;
    }
}
