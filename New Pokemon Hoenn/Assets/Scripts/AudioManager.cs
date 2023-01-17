using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [System.Serializable]
    public class Sound{
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public void PlaySoundEffect(Sound sound){
        soundEffectSource.volume = sound.volume;
        soundEffectSource.PlayOneShot(sound.clip);
    }

    void Awake(){
        if(Instance != null){
            Debug.Log("AudioManager instance exists");
            return;
        }
        Instance = this;
    }
}
