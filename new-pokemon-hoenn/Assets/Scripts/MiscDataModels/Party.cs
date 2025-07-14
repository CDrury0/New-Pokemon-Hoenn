using UnityEngine;
using System.Collections.Generic;

public class Party
{
    public const int PARTY_SIZE = 6;
    public List<Pokemon> members;

    public Party(){
        members = new(PARTY_SIZE);
    }

    public Party(Pokemon p) : this() {
        members.Add(p);
    }

    public Party(IEnumerable<Pokemon> createFrom): this() {
        members.AddRange(createFrom);
    }

    public Party(SerializablePokemon[] templateToCopy) : this() {
        foreach(var entry in templateToCopy)
            if(entry is not null)
                members.Add(new Pokemon(entry));
    }

    //add methods to retrieve info e.g. leader ability

    public bool IsFull() => members.Count == members.Capacity;

    public Pokemon GetFirstAvailable(){
        foreach(var p in members)
            if(!p.IsFainted() && !p.inBattle){
                p.inBattle = true;
                return p;
            }

        return null;
    }

    public bool HasAvailableFighter() => members.Find((p) => p.IsAvailableFighter()) is not null;

    public bool IsEntireTeamFainted(){
        return members.Find((p) => !p.IsFainted()) is null;
    }
}
