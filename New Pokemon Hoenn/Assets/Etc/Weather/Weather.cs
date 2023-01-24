using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weather : ScriptableObject
{
    public StatLib.Type typeFromWeather;
    public StatLib.Type weakensType;
    public string textOnSet;
    public string textOnContinue;
    public string textOnStop;
    public bool damageEveryTurn;
    public string textOnDamage;
    public StatLib.Type[] immuneTypes;
    public bool healsMore;
    public bool healsLess;
}
