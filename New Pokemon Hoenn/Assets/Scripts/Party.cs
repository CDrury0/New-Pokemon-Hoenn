using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public Pokemon[] party = new Pokemon[6];

    public Party(){
        party = new Pokemon[6];
    }

    public Party(SerializablePokemon[] templateToCopy){
        for(int i = 0; i < party.Length; i++){
            if(templateToCopy[i] != null){
                party[i] = new Pokemon(templateToCopy[i]);
            }
            else{
                party[i] = null;
            }
        }
    }

    //add methods to retrieve info e.g. leader ability
    public Pokemon GetFirstAvailable(){
        foreach (Pokemon p in party){
            if(p != null && p.primaryStatus != PrimaryStatus.Fainted && !p.inBattle){
                return p;
            }
        }
        return null;
    }
}
