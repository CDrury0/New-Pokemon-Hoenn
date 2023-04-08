using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weather : ScriptableObject
{
    public Pokemon.Type typeFromWeather;
    public Pokemon.Type weakensType;
    public string textOnSet;
    public string textOnContinue;
    public string textOnStop;
    public bool damageEveryTurn;
    public string textOnDamage;
    public Pokemon.Type[] immuneTypes;
    public bool healsMore;
    public bool healsLess;
}
