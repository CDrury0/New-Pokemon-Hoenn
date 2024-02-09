using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference Objects/ReferenceLib")]
public class ReferenceLib : ScriptableObject
{
    public static ReferenceLib Instance { get; private set; }
    public List<PokemonDefault> pokemonDefaultLib;
    public List<TypeMatchupList> typeEffectivenessMatchups;
    public AreaData activeArea;

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

    public void Awake() {
        if(Instance != null){
            Debug.LogWarning("Multiple ReferenceLib Objects Detected");
            return;
        }
        Instance = this;
    }
}
