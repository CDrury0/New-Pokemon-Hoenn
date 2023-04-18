using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceLib : MonoBehaviour
{
    public static ReferenceLib Instance {get; private set;}
    public PokemonDefault[] pokemonDefaultLib;
    public List<TypeMatchupList> typeEffectivenessMatchups;
    public AreaData activeArea;

    void Awake(){
        if(Instance != null){
            Debug.Log("PokemonReferenceLib exists");
            return;
        }
        Instance = this;
    }

    [System.Serializable]
    public class TypeMatchupList{
        public Pokemon.Type attackingType;
        public List<TypeMatchupValues> matchup;
    }

    [System.Serializable]
    public class TypeMatchupValues{
        public Pokemon.Type defendingType;
        public StatLib.Matchup effectiveness;
    }
}
