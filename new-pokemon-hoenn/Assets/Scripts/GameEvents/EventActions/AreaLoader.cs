using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is necessary because the GameAreaManager will sometimes not be instantiated, meaning a coroutine cannot be reliably started from it
/// </summary>
public class AreaLoader : EventAction
{
    [SerializeField] private bool loadLastHealArea;
    [SerializeField] private bool loadEscapeArea;
    [SerializeField] private AreaData areaDataOverride;

    protected override IEnumerator EventActionLogic() {
        GameAreaManager areaEntered = GetAreaToLoad();
        if(areaEntered.areaData == ReferenceLib.ActiveArea){
            exit = true;
            yield break;
        }
        exit = false;
        StartCoroutine(areaEntered.LoadArea());
    }

    private GameAreaManager GetAreaToLoad() {
        GameObject prefabToLoad = null;
        if(loadLastHealArea){
            prefabToLoad = ReferenceLib.LastHealPosition.key.areaObjectPrefab;
        }
        if(loadEscapeArea){
            prefabToLoad = ReferenceLib.EscapePosition.key.areaObjectPrefab;
        }
        if(areaDataOverride != null){
            prefabToLoad = areaDataOverride.areaObjectPrefab;
        }
            
        return prefabToLoad?.GetComponent<GameAreaManager>() ?? GetComponentInParent<GameAreaManager>();
    }
}
