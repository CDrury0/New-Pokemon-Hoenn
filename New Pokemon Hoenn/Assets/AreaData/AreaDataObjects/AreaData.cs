using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class AreaData : ScriptableObject
{
    public string areaName;
    public GameObject areaObjectPrefab;
    public List<GameObject> adjacentObjectPrefabs;
    public GameObject eventObjectPrefab;
    public TerrainDataSO terrainData;
    public Weather weather;
    public GenerationValues generationValues;
    public AudioClip musicIntro;
    public AudioClip musicLoop;
    public List<int> eventManifest;
}
