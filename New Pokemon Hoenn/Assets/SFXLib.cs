using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLib : MonoBehaviour
{
    public static SFXLib Instance { get; private set; }
    public AudioClip notVeryEffective;
    public AudioClip normalEffective;
    public AudioClip superEffective;

    void Awake() {
        if(Instance != null){
            Debug.Log("SFXLib already exists!");
            return;
        }
        Instance = this;
    }
}
