using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    private IEnumerator currentMusicCycle;

    public void PlaySoundEffect(AudioClip sound, bool reduceMusic){
        if(reduceMusic){
            StartCoroutine(DoSoundEffectWithFade(sound));
            return;
        }
        PlaySoundEffect(sound);
    }

    private IEnumerator DoSoundEffectWithFade(AudioClip sound){
        float musicStartingVolume = PlayerPrefs.GetFloat("musicVolume");
        yield return StartCoroutine(FadeSound(musicStartingVolume, musicStartingVolume * 0.2f, 0.5f));
        PlaySoundEffect(sound);
        yield return new WaitForSeconds(sound.length);
        yield return StartCoroutine(FadeSound(musicSource.volume, musicStartingVolume, 0.5f));
    }

    private IEnumerator FadeSound(float start, float end, float timeSeconds){
        float elapsedTime = 0;
        while(elapsedTime < timeSeconds){
            elapsedTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(start, end, elapsedTime / timeSeconds);
            yield return null;
        }
    }

    private void PlaySoundEffect(AudioClip sound){
        soundEffectSource.volume = PlayerPrefs.GetFloat("effectVolume");
        soundEffectSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip intro, AudioClip loop, bool fadeOutMusic){
        if(fadeOutMusic){
            StartCoroutine(FadeMusic(intro, loop));
            return;
        }
        PlayMusic(intro, loop);
    }

    private void PlayMusic(AudioClip intro, AudioClip loop){
        if(currentMusicCycle != null){
            StopCoroutine(currentMusicCycle);
            //Get rid of unused AudioSource components 
            //(extras are created if the music is changed before the loop source can be assigned)
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource a in audioSources){
                if(a != musicSource && a != soundEffectSource){
                    Destroy(a);
                }
            }
        }
        musicSource.Stop();

        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        currentMusicCycle = DoMusicCycle(intro, loop);
        StartCoroutine(currentMusicCycle);
    }

    private IEnumerator FadeMusic(AudioClip intro, AudioClip loop){
        yield return StartCoroutine(FadeSound(musicSource.volume, 0f, 1.2f));
        PlayMusic(intro, loop);
    }

    private IEnumerator DoMusicCycle(AudioClip introClip, AudioClip loopClip){
        yield return new WaitForSeconds(0.1f);
        musicSource.clip = introClip;
        musicSource.loop = false;
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.clip = loopClip;
        newSource.loop = true;
        musicSource.Play();
        yield return new WaitUntil(() => !musicSource.isPlaying);
        newSource.Play();
        //waiting one frame reduces audio lag, presumably because it delays the destroy call until after the source is playing
        yield return null;
        Destroy(musicSource);
        musicSource = newSource;
    }

    void Awake(){
        if(Instance != null){
            Debug.Log("AudioManager instance exists");
            return;
        }
        Instance = this;
    }
}
