using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSwitch : EventAction
{
    [SerializeField] private EventCondition condition;
    [SerializeField] protected UnityEvent<EventState> alternateEvent;

    public override void DoEventAction(EventState chainState) {
        var eventToInvoke = condition.IsConditionTrue(chainState) ? nextEvent : alternateEvent;
        NextAction(chainState, eventToInvoke);
    }
}
