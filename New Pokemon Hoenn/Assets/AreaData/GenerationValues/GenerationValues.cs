using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GenerationValues : ScriptableObject
{
    public SpawnInfo[] grassSpawnInfo;
    public SpawnInfo[] surfSpawnInfo;
    public SpawnInfo[] oldRodSpawnInfo;
    public SpawnInfo[] goodRodSpawnInfo;
    public SpawnInfo[] superRodSpawnInfo;
}

[System.Serializable]
public class SpawnInfo
{
    public PokemonDefault pokemonDefault;
    public float spawnRate;
    public int[] levelRange = new int[2];
}
