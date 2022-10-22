using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType {Normal, Desert}
[CreateAssetMenu]
public class AreaData : ScriptableObject
{
    public string areaName;
    public TerrainType terrainType;
    public Weather weather;
    public GenerationValues generationValues;
    public AudioClip musicIntro;
    public AudioClip musicLoop;
}
