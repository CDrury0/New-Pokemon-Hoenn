using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventStateReceiver<T> : EventAction
{
    [SerializeField] protected EventAction stateSender;

    protected IEventStateSender<T> GetStateSender() {
        return stateSender as IEventStateSender<T>;
    }
}
