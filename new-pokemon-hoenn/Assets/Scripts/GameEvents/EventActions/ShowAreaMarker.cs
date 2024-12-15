using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAreaMarker : EventAction
{
    [SerializeField] private GameObject areaMarkerPrefab;
    
    public override void DoEventAction(EventState chainState) {
        Instantiate(areaMarkerPrefab);
        NextAction(chainState);
    }
}
