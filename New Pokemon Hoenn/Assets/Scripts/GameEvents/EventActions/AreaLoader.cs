using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is necessary because the GameAreaManager will sometimes not be instantiated, meaning a coroutine cannot be reliably started from it
/// </summary>
public class AreaLoader : EventAction
{
    private GameAreaManager areaEntered;

    protected override IEnumerator EventActionLogic() {
        if(areaEntered.areaData == ReferenceLib.Instance.activeArea){
            exit = true;
            yield break;
        }
        exit = false;
        StartCoroutine(areaEntered.LoadArea());
    }

    void Start() {
        areaEntered = GetComponentInParent<GameAreaManager>();
    }
}
