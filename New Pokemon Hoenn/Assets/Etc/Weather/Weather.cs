using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weather : ScriptableObject
{
    public PokemonType typeFromWeather;
    public PokemonType weakensType;
    public string textOnSet;
    public string textOnContinue;
    public string textOnStop;
    public bool damageEveryTurn;
    public string textOnDamage;
    public PokemonType[] immuneTypes;
    public bool healsMore;
    public bool healsLess;
}
