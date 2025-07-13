using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCondition : ScriptableObject
{
    public abstract bool IsConditionTrue();
}
