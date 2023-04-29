using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkEventComplete : EventAction
{
    [SerializeField] private EventTrigger toMarkComplete;
    protected override IEnumerator EventActionLogic() {
        GetComponentInParent<GameAreaManager>().areaData.eventManifest.Add(toMarkComplete.EventTriggerID);
        yield break;
    }

    void Reset() {
        toMarkComplete = gameObject.GetComponent<EventTrigger>();
    }
}
