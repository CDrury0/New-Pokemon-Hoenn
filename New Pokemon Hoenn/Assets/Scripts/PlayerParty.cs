using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static PlayerParty Instance {get; private set;}
    public Party playerParty;

    void Awake(){
        if(Instance != null){
            Debug.Log("PlayerPartyInstance exists");
            return;
        }
        Instance = this;
    }

    //include methods to retrieve specific player party info, like leader ability, etc.
}
