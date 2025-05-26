using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSwitch : EventAction
{
    [SerializeField] private EventCondition condition;
    [SerializeField] private EventAction falseEvent;

    protected override IEnumerator EventActionLogic(EventState state) {
        if(condition.IsConditionTrue()){
            StartCoroutine(nextEvent.DoEventAction(state));
            yield break;
        }
        StartCoroutine(falseEvent.DoEventAction(state));
    }
}
