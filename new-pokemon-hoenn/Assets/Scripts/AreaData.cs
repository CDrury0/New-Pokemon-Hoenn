using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class AreaData : ScriptableObject
{
    public const string RESOURCE_PATH = "AreaDataObjects/";
    public string areaName;
    public GameObject areaObjectPrefab;
    public List<GameObject> adjacentObjectPrefabs;
    [Tooltip("The event container prefab that should be a child of this area object prefab")]
    public GameObject eventObjectPrefab;
    public TerrainDataSO terrainData;
    public Weather weather;
    public GenerationValues defaultGenerationValues;
    public AudioClip musicIntro;
    public AudioClip musicLoop;
    public List<int> eventManifest;

    /// <summary>
    /// Must be the ScriptableObject name, not this.areaName
    /// </summary>
    public static AreaData GetAreaFromName(string name) => Resources.Load<AreaData>(RESOURCE_PATH + name);
}
