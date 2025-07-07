using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSwitch : EventAction, IEventStateSender
{
    [SerializeField] private EventCondition condition;
    [SerializeField] private EventAction falseEvent;

    public List<T> GetState<T>() where T : class {
        List<T> list = new();
        var state = condition as T;
        if(state is not null)
            list.Add(state);

        return list;
    }

    protected override IEnumerator EventActionLogic() {
        if(condition.IsConditionTrue()){
            StartCoroutine(nextEvent.DoEventAction());
            yield break;
        }
        StartCoroutine(falseEvent.DoEventAction());
    }
}
