using UnityEngine;
using System.Collections.Generic;

public class Party
{
    public Pokemon[] party;

    public Party(){
        party = new Pokemon[6];
    }

    public Party(Pokemon p) : this() {
        party[0] = p;
    }

    public Party(SerializablePokemon[] templateToCopy) : this() {
        for(int i = 0; i < templateToCopy.Length; i++){
            if(templateToCopy[i] != null){
                party[i] = new Pokemon(templateToCopy[i]);
            }
        }
    }

    //add methods to retrieve info e.g. leader ability

    /// <summary>
    /// Returns the index of the first empty slot, or -1 if the party is full
    /// </summary>
    public int PartyIsFull(){
        for (int i = 0; i < party.Length; i++){
            if(party[i] == null){
                return i;
            }
        }
        return -1;
    }

    public Pokemon GetFirstAvailable(){
        foreach (Pokemon p in party){
            if(p != null && p.primaryStatus != PrimaryStatus.Fainted && !p.inBattle){
                p.inBattle = true;
                return p;
            }
        }
        return null;
    }

    public bool HasAvailableFighter(){
        foreach(Pokemon p in party){
            if(CheckIsAvailableFighter(p)){
                return true;
            }
        }
        return false;
    }

    public static bool CheckIsAvailableFighter(Pokemon p){
        return p != null && p.primaryStatus != PrimaryStatus.Fainted && !p.inBattle && !CombatLib.Instance.combatSystem.IsRegisteredToSwitchIn(p);
    }

    public bool IsEntireTeamFainted(){
        foreach(Pokemon p in party){
            if(p != null && p.primaryStatus != PrimaryStatus.Fainted){
                return false;
            }
        }
        return true;
    }
}
