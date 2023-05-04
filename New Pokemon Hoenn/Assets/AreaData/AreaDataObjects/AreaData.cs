using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class AreaData : ScriptableObject
{
    public string areaName;
    public GameObject areaObjectPrefab;
    [Tooltip("Adjacency list should contain a reference to THIS game area as well")]
    public List<GameObject> adjacentObjectPrefabs;
    [Tooltip("The event container prefab that should be a child of this area object prefab")]
    public GameObject eventObjectPrefab;
    public TerrainDataSO terrainData;
    public Weather weather;
    public GenerationValues generationValues;
    public AudioClip musicIntro;
    public AudioClip musicLoop;
    public List<int> eventManifest;
}
