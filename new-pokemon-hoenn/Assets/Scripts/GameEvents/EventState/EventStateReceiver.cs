using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventStateReceiver<T> : EventAction, IStateReceiverUpdatable where T : class
{
    [SerializeField] protected EventAction stateSender;

    protected List<T> GetSenderState() {
        return (stateSender as IEventStateSender)?.SendState<T>();
    }

    /// <summary>
    /// Sets the stateSender of this if it was not assigned in the editor
    /// </summary>
    public void SetStateSender(IEventStateSender sender) {
        stateSender ??= sender as EventAction;
    }
}
