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
    public List<PokemonType> typeList;
    [SerializeField] private List<PokemonNature> natures;
    public AreaData activeArea;

    public static PokemonType GetPokemonType(string typeName){
        return Instance.typeList.Find(t => t.name == typeName);
    }

    public static List<PokemonNature> GetNatures(){
        return Instance.natures;
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

    public static void UpdateDexStatus(PokemonDefault mon, DexStatus status) {
        int index = mon.IDNumber;
        if((int)status < (int)GlobalDexProgress[index]){
            return;
        }
        GlobalDexProgress[index] = status;
    }
}
