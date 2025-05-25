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
    [SerializeField] private List<GymBadge> gymBadgeReference;
    [SerializeField] private List<PokemonNature> natures;
    public static ItemData FallbackBall => Instance._fallbackBall;
    [SerializeField] private ItemData _fallbackBall;
    public static AreaData ActiveArea {
        get => Instance._activeArea;
        set => Instance._activeArea = value;
    }
    [SerializeField] private AreaData _activeArea;
    public static DynamicDictionary<AreaData, Vector3>.Entry LastHealPosition {
        get => Instance._lastHealPosition;
        private set => Instance._lastHealPosition = value;
    }
    [SerializeField] private DynamicDictionary<AreaData, Vector3>.Entry _lastHealPosition;
    public static DynamicDictionary<AreaData, Vector3>.Entry EscapePosition {
        get => Instance._escapePosition;
        set => Instance._escapePosition = value;
    }
    [SerializeField] private DynamicDictionary<AreaData, Vector3>.Entry _escapePosition;

    public static List<GymBadge> PlayerBadges {
        get => Instance._playerBadges;
        private set => Instance._playerBadges = value;
    }
    [SerializeField] private List<GymBadge> _playerBadges;

    public static void SetAllowMenuToggle(bool val) => PlayerInput.AllowMenuToggle = val;
    public static PokemonType GetPokemonType(string typeName) => Instance.typeList.Find(t => t.name == typeName);
    public static List<PokemonNature> GetNatures() => Instance.natures;
    public static GymBadge GetGymBadge(string badgeName) => Instance.gymBadgeReference.Find((badge) => badge.BadgeName == badgeName);

    public static void SetLastHealPosition() {
        Instance._lastHealPosition.key = ActiveArea;
        Instance._lastHealPosition.value = PlayerInput.playerTransform.position;
    }

    public void Init() {
        if(Instance != null){
            Debug.LogWarning("Multiple ReferenceLib Objects Detected");
            return;
        }
        Instance = this;

        pokemonLibByID = pokemonDefaultLib.OrderBy(p => p.IDNumber).ToList();
        pokemonLibByID.Insert(0, null);

        GlobalDexProgress = SaveManager.LoadedSave?.dexStatus ?? new DexStatus[pokemonDefaultLib.Count + 1];
        if(SaveManager.LoadedSave?.currentPosition != null){
            ActiveArea = AreaData.GetAreaFromName(SaveManager.LoadedSave.currentPosition.key);
        }
        if(SaveManager.LoadedSave?.lastHealedPosition != null){
            AreaData area = AreaData.GetAreaFromName(SaveManager.LoadedSave.lastHealedPosition.key);
            LastHealPosition = new(area, SaveManager.LoadedSave.lastHealedPosition.value);
        }
        if(SaveManager.LoadedSave?.escapePosition != null){
            AreaData area = AreaData.GetAreaFromName(SaveManager.LoadedSave.escapePosition.key);
            EscapePosition = new(area, SaveManager.LoadedSave.escapePosition.value);
        }
        if(SaveManager.LoadedSave?.gameAreaEventManifests != null){
            foreach(var entry in SaveManager.LoadedSave.gameAreaEventManifests){
                AreaData area = AreaData.GetAreaFromName(entry.key);
                if (area?.eventManifest != null){
                    area.eventManifest = entry.value.ToList();
                }
            }
        }
        if(SaveManager.LoadedSave?.playerBadges != null){
            _playerBadges = SaveManager.LoadedSave.playerBadges.Select((badgeName) => GetGymBadge(badgeName)).ToList();
        }
    }

    public static void UpdateDexStatus(PokemonDefault mon, DexStatus status) {
        int index = mon.IDNumber;
        if((int)status > (int)GlobalDexProgress[index]){
            GlobalDexProgress[index] = status;
        }
    }
}
