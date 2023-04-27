using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkEventComplete : InteractEvent
{
    [SerializeField] private EventTrigger toMarkComplete;
    protected override IEnumerator InteractEventLogic() {
        GetComponentInParent<GameAreaManager>().completedEvents.Add(toMarkComplete.EventTriggerID);
        yield break;
    }

    void Reset() {
        toMarkComplete = gameObject.GetComponent<EventTrigger>();
    }
}
