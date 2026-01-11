using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static PlayerParty Instance {get; private set;}
    public static Party Party {get => Instance.playerParty;}
    public Party playerParty;
    public bool startWithParty;
    public SerializablePokemon[] startingParty;

    //include methods to retrieve specific player party info, like leader ability, etc.

    // Get first pokemon that isn't an egg?
    public static Pokemon GetLeader() => Party.members.FirstOrDefault();

    /// <returns>Whether the Pokemon was actually put in the player's party (true) or was sent to the box (false)</returns>
    public bool GivePlayerPokemon(Pokemon p){
        // Update DexStatus to caught
        ReferenceLib.UpdateDexStatus(p.pokemonDefault, DexStatus.Caught);
        if(!playerParty.IsFull()){
            playerParty.members.Add(p);
            return true;
        }

        //send pokemon to box
        return false;
    }

    void Awake(){
        if(Instance != null){
            Debug.LogWarning("Multiple PlayerParty instances exist");
            return;
        }
        Instance = this;
    }

    private void Start(){
        playerParty = new Party();

        if(startWithParty){
            playerParty = new Party(startingParty);
            return;
        }

        if(SaveManager.LoadedSave?.currentParty != null){
            int len = Mathf.Min(SaveManager.LoadedSave.currentParty.Count, playerParty.members.Capacity);
            for(int i = 0; i < len; i++)
                playerParty.members.Add(new Pokemon(SaveManager.LoadedSave.currentParty[i]));
        }
    }
}
