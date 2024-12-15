using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventAction : MonoBehaviour
{
    [SerializeField] protected bool destroySelfOnComplete;
    //[SerializeField] protected EventAction nextEvent;
    [SerializeField] protected UnityEvent<EventState> nextEvent;
    protected bool exit;
    public abstract void DoEventAction(EventState chainState);
    protected virtual void NextAction(EventState chainState, UnityEvent<EventState> toInvoke = null) {
        toInvoke ??= nextEvent;
        if(toInvoke.GetPersistentEventCount() == 0 || exit) {
            PlayerInput.allowMovementInput = true;
            PlayerInput.AllowMenuToggle = true;
        }
        else {
            toInvoke?.Invoke(chainState);
        }

        if(destroySelfOnComplete)
            Destroy(gameObject);
    }

    protected IEnumerator ChainGang(IEnumerator coroutine, System.Action after) {
        yield return StartCoroutine(coroutine);
        after?.Invoke();
    }
}
