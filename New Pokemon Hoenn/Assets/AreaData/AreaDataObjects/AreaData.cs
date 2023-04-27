using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaData : ScriptableObject
{
    public string areaName;
    public TerrainDataSO terrainData;
    public Weather weather;
    public GenerationValues generationValues;
    public AudioClip musicIntro;
    public AudioClip musicLoop;
    public List<int> eventManifest;
}
