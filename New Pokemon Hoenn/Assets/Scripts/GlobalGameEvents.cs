using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterType { Grass, Surf, Rod_Old, Rod_Good, Rod_Super}

[System.Serializable]
public static class GlobalGameEvents
{
    public static float[] playerPosition;
    public static List<Item> globalPlayerInventory;
    public static DexStatus[] globalDexProgress = new DexStatus[215];

    public static float GetBattleEncounterChance()
    {
        return 1f;
    }
}
