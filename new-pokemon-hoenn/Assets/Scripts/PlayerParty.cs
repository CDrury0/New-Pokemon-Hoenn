using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static PlayerParty Instance {get; private set;}
    public Party playerParty;
    public PokemonDefault starterToGive;

    //include methods to retrieve specific player party info, like leader ability, etc.

    // Get first pokemon that isn't an egg?
    public static Pokemon GetLeader() => Instance.playerParty.party[0];

    /// <returns>Whether the Pokemon was actually put in the player's party (true) or was sent to the box (false)</returns>
    public bool GivePlayerPokemon(Pokemon p){
        // Update DexStatus to caught
        ReferenceLib.UpdateDexStatus(p.pokemonDefault, DexStatus.Caught);

        int index = playerParty.GetFirstEmpty();
        if(index != -1){
            playerParty.party[index] = p;
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

        if(starterToGive is not null){
            playerParty = new Party(new Pokemon(starterToGive, 13));
        }

        if(SaveManager.LoadedSave?.currentParty != null){
            int len = Mathf.Min(SaveManager.LoadedSave.currentParty.Count, playerParty.party.Length);
            for(int i = 0; i < len; i++){
                playerParty.party[i] = new Pokemon(SaveManager.LoadedSave.currentParty[i]);
            }
        }
    }
}
