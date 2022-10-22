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
            party[i] = new Pokemon(partyToCopy.party[i]);
        }
    }

    //add methods to retrieve info e.g. leader ability
    public Pokemon GetFirstAvailable(){
        foreach (Pokemon p in party){
            if(p.assigned && p.primaryStatus != PrimaryStatus.Fainted && !p.inBattle){
                return p;
            }
        }
        return null;
    }
}
