using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<PokemonDefault> pokemonLibByID;
    public List<GameObject> moveManifest;
    public List<PokemonType> typeList;
    [SerializeField] private List<PokemonNature> natures;
    public static ItemData FallbackBall => Instance._fallbackBall;
    [SerializeField] private ItemData _fallbackBall;
    public static AreaData ActiveArea {
        get => Instance._activeArea;
        set => Instance._activeArea = value;
    }
    [SerializeField] private AreaData _activeArea;
    public static DynamicDictionary<AreaData, Vector3>.Entry LastHealPosition => Instance._lastHealPosition;
    [SerializeField] private DynamicDictionary<AreaData, Vector3>.Entry _lastHealPosition;
    public static DynamicDictionary<AreaData, Vector3>.Entry EscapePosition {
        get => Instance._escapePosition;
        set => Instance._escapePosition = value;
    }
    [SerializeField] private DynamicDictionary<AreaData, Vector3>.Entry _escapePosition;

    public static PokemonType GetPokemonType(string typeName) => Instance.typeList.Find(t => t.name == typeName);
    public static List<PokemonNature> GetNatures() => Instance.natures;

    public static void SetLastHealPosition() {
        Instance._lastHealPosition.key = ActiveArea;
        Instance._lastHealPosition.value = PlayerInput.playerTransform.position;
    }

    public void Awake() {
        if(Instance != null){
            Debug.LogWarning("Multiple ReferenceLib Objects Detected");
            return;
        }
        Instance = this;

        GlobalDexProgress ??= new DexStatus[pokemonDefaultLib.Count + 1];
        pokemonLibByID = pokemonDefaultLib.OrderBy(p => p.IDNumber).ToList();
    }

    public static void UpdateDexStatus(PokemonDefault mon, DexStatus status) {
        int index = mon.IDNumber;
        if((int)status > (int)GlobalDexProgress[index]){
            GlobalDexProgress[index] = status;
        }
    }
}
