using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventUnityAction : EventAction
{
    [SerializeField] private UnityEvent action;

    protected override IEnumerator EventActionLogic() {
        action?.Invoke();
        yield break;
    }
}
