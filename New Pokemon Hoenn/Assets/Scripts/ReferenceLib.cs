using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference Objects/ReferenceLib")]
public class ReferenceLib : ScriptableObject
{
    public static ReferenceLib Instance { get; private set; }
    /// <summary>
    /// Lookup index should equal Pokemon ID
    /// </summary>
    public static DexStatus[] GlobalDexProgress;
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
        
        // Load dex progress from save
        GlobalDexProgress ??= new DexStatus[pokemonDefaultLib.Count + 1];
        // Give caught status to every mon
        for (int i = 1; i < GlobalDexProgress.Length; i++){
            GlobalDexProgress[i] = DexStatus.Caught;
        }
    }
}
