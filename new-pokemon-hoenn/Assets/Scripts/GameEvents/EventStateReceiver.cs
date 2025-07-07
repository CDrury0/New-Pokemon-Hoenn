using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventStateReceiver<T> : EventAction where T : class
{
    [SerializeField] protected EventAction stateSender;

    protected List<T> GetSenderState() {
        return (stateSender as IEventStateSender)?.GetState<T>();
    }
}
