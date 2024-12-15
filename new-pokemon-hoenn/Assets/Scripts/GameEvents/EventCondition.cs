using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EventCondition : MonoBehaviour
{
    public abstract bool IsConditionTrue();
    public abstract bool IsConditionTrue(EventState state);
}
