using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAreaMarker : EventAction
{
    [SerializeField] private GameObject areaMarkerPrefab;
    protected override IEnumerator EventActionLogic(EventState state) {
        Instantiate(areaMarkerPrefab);
        yield break;
    }
}
