using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkEventComplete : EventAction
{
    [SerializeField] private EventTrigger[] toMarkComplete;

    public override void DoEventAction(EventState chainState) {
        AreaData thisAreaData = GetComponentInParent<GameAreaManager>().areaData;
        for (int i = 0; i < toMarkComplete.Length; i++)
            if (!thisAreaData.eventManifest.Contains(toMarkComplete[i].EventTriggerID))
                thisAreaData.eventManifest.Add(toMarkComplete[i].EventTriggerID);

        NextAction(chainState);
    }
}
