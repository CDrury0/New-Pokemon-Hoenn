using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Generation Values")]
public class GenerationValues : ScriptableObject
{
    /// <summary>
    /// SpawnInfo structs that do not pass a predicate are not considered for Pokemon generation
    /// </summary>
    public static List<System.Predicate<SpawnInfo>> spawnFilters = new();

    /// <summary>
    /// Each function provided modifies the properties of the SpawnInfo structs considered for Pokemon generation
    /// </summary>
    public static List<System.Func<SpawnInfo, SpawnInfo>> spawnInfoModifiers = new();
    [SerializeField] private List<SpawnInfo> spawnInfo;
    
    public Pokemon GeneratePokemon() {
        List<SpawnInfo> spawnInfo = FilterSpawns();
        if(spawnInfo.Count == 0){
            return null;
        }
        float rand = Random.Range(0f, 1f);
        float spawnRateCounter = 0f;
        for(int i = 0; i < spawnInfo.Count; i++){
            SpawnInfo entry = spawnInfo[i];
            if(rand < entry.spawnRate + spawnRateCounter){
                return new Pokemon(entry.pokemonDefault, entry.GetRandomLevel());
            }
            spawnRateCounter += entry.spawnRate;
        }
        // Automatically use the final entry if the random float overflows (astronomically rare, but not impossible)
        SpawnInfo fallback = spawnInfo[^1];
        return new Pokemon(fallback.pokemonDefault, fallback.GetRandomLevel());
    }

    private List<SpawnInfo> FilterSpawns(){
        List<SpawnInfo> filtered = new(spawnInfo);
        foreach(System.Predicate<SpawnInfo> filter in spawnFilters){
            filtered = filtered.FindAll(filter);
        }
        for(int i = 0; i < filtered.Count; i++){
            foreach(System.Func<SpawnInfo, SpawnInfo> func in spawnInfoModifiers){
                filtered[i] = func(filtered[i]);
            }
        }
        filtered = NormalizeSpawnRates(filtered);

        return filtered;
    }

    private List<SpawnInfo> NormalizeSpawnRates(List<SpawnInfo> spawnInfo){
        float totalRate = spawnInfo.Sum(info => info.spawnRate);
        float multiplier = 1f / totalRate;
        List<SpawnInfo> newSpawns = new(spawnInfo);
        for(int i = 0; i < spawnInfo.Count; i++){
            newSpawns[i] = newSpawns[i].UpdateSpawnRate(newSpawns[i].spawnRate * multiplier);
        }
        return newSpawns;
    }

    public int GetHighestLevel() {
        return spawnInfo.OrderBy(info => info.levelMax).Last().levelMax;
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public PokemonDefault pokemonDefault;
    public float spawnRate;
    public int levelMin;
    public int levelMax;

    public SpawnInfo(PokemonDefault pokemonDefault, float spawnRate, int levelMin, int levelMax){
        this.pokemonDefault = pokemonDefault;
        this.spawnRate = spawnRate;
        this.levelMin = levelMin;
        this.levelMax = levelMax;
    }

    public SpawnInfo UpdatePokemonDefault(PokemonDefault newDefault){
        return new SpawnInfo(newDefault, spawnRate, levelMin, levelMax);
    }

    public SpawnInfo UpdateSpawnRate(float newRate){
        return new SpawnInfo(pokemonDefault, newRate, levelMin, levelMax);
    }

    public SpawnInfo UpdateMinLevel(int newLevel){
        return new SpawnInfo(pokemonDefault, spawnRate, newLevel, levelMax);
    }

    public SpawnInfo UpdateMaxLevel(int newLevel){
        return new SpawnInfo(pokemonDefault, spawnRate, levelMin, newLevel);
    }

    public int GetRandomLevel() {
        return Random.Range(levelMin, levelMax + 1);
    }
}
