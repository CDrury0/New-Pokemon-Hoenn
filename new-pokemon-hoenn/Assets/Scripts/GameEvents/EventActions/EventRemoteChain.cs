using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRemoteChain : EventAction {
    public GameObject prefab;

    protected override IEnumerator EventActionLogic() {
        var instance = Instantiate(prefab);
        nextEvent = instance.GetComponent<EventAction>();
        yield break;
    }
}
