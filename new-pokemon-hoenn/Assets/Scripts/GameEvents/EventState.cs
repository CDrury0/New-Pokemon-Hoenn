using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/EventState")]
public class EventState : ScriptableObject
{
    public const int DEFAULT_VALUE = -1;
    public int value = DEFAULT_VALUE;

    public bool IsUnset() => value == DEFAULT_VALUE;

    public void ResetVal() => value = DEFAULT_VALUE;
}
