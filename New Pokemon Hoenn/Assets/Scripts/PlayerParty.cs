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

    /// <returns>Whether the Pokemon was actually put in the player's party (true) or was sent to the box (false)</returns>
    public bool GivePlayerPokemon(Pokemon p){
        int index = playerParty.GetFirstEmpty();
        if(index != -1){
            playerParty.party[index] = p;
            return true;
        }

        //send pokemon to box
        return false;
    }

    //include methods to retrieve specific player party info, like leader ability, etc.
}
