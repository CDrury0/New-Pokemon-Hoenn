using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    private IEnumerator currentMusicCycle;

    [System.Serializable]
    public class Sound{
        public AudioClip clip;
    }

    public void PlaySoundEffect(Sound sound, bool reduceMusic){
        if(reduceMusic){
            StartCoroutine(DoSoundEffectWithFade(sound));
            return;
        }
        PlaySoundEffect(sound);
    }

    private IEnumerator DoSoundEffectWithFade(Sound sound){
        float startingVolume = musicSource.volume;
        yield return StartCoroutine(FadeSound(startingVolume, startingVolume * 0.2f, 0.5f));
        PlaySoundEffect(sound);
        yield return new WaitForSeconds(sound.clip.length);
        yield return StartCoroutine(FadeSound(musicSource.volume, startingVolume, 0.5f));
    }

    private IEnumerator FadeSound(float start, float end, float timeSeconds){
        float elapsedTime = 0;
        while(elapsedTime < timeSeconds){
            elapsedTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(start, end, elapsedTime / timeSeconds);
            yield return null;
        }
    }

    private void PlaySoundEffect(Sound sound){
        soundEffectSource.PlayOneShot(sound.clip);
    }

    public void PlayMusic(Sound intro, Sound loop, bool fadeOutMusic){
        if(fadeOutMusic){
            StartCoroutine(FadeMusic(intro, loop));
            return;
        }
        PlayMusic(intro, loop);
    }

    private void PlayMusic(Sound intro, Sound loop){
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

        //replace this and other volume sets to take from PlayerPrefs
        musicSource.volume = 1f;
        currentMusicCycle = DoMusicCycle(intro, loop);
        StartCoroutine(currentMusicCycle);
    }

    private IEnumerator FadeMusic(Sound intro, Sound loop){
        yield return StartCoroutine(FadeSound(musicSource.volume, 0f, 2f));
        PlayMusic(intro, loop);
    }

    private IEnumerator DoMusicCycle(Sound intro, Sound loop){
        yield return new WaitForSeconds(0.1f);
        musicSource.clip = intro.clip;
        musicSource.loop = false;
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.clip = loop.clip;
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
