using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Party
{
    public Pokemon[] party = new Pokemon[6];

    public Party(){
        party = new Pokemon[6];
    }

    public Party(Party partyToCopy){
        for(int i = 0; i < party.Length; i++){
            if(partyToCopy.party[i] != null && !partyToCopy.party[i].assigned){
                party[i] = new Pokemon(partyToCopy.party[i]);
            }
            else{
                party[i] = null;
            }
        }
    }

    //add methods to retrieve info e.g. leader ability
    public Pokemon GetFirstAvailable(){
        foreach (Pokemon p in party){
            if(p != null && p.assigned && p.primaryStatus != PrimaryStatus.Fainted && !p.inBattle){
                return p;
            }
        }
        return null;
    }
}
