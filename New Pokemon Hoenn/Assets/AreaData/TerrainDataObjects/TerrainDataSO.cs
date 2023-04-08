using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrainDataSO : ScriptableObject
{
    //battle background sprite, pokemon team circle sprite, secret power type, etc.
    public Sprite battleBackgroundSprite;
    public Sprite pokemonSpawnCircleSprite;
    public Pokemon.Type secretPowerType;
}
