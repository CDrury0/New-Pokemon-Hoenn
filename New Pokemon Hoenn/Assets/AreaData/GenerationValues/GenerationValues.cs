using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Generation Values")]
public class GenerationValues : ScriptableObject
{
    public SpawnInfo[] spawnInfo;
    
    public Pokemon GeneratePokemon() {
        float rand = Random.Range(0f, 1f);
        float spawnRateCounter = 0f;
        for(int i = 0; i < spawnInfo.Length; i++){
            SpawnInfo entry = spawnInfo[i];
            if(rand < entry.spawnRate + spawnRateCounter){
                return new Pokemon(entry.pokemonDefault, entry.GetLevel());
            }
            spawnRateCounter += entry.spawnRate;
        }
        // Automatically use the final entry if the random float overflows (astronomically rare, but not impossible)
        SpawnInfo fallback = spawnInfo[^1];
        return new Pokemon(fallback.pokemonDefault, fallback.GetLevel());
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public PokemonDefault pokemonDefault;
    public float spawnRate;
    public int levelMin;
    public int levelMax;

    public int GetLevel() {
        return Random.Range(levelMin, levelMax + 1);
    }
}
