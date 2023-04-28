using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaManager : MonoBehaviour
{
    public AreaData areaData;

    public IEnumerator LoadArea() {
        if(areaData == ReferenceLib.Instance.activeArea){
            yield break;
        }
        Debug.Log("Loading " + areaData.areaName);
        ReferenceLib.Instance.activeArea = areaData;
        List<GameAreaManager> allLoadedAreas = new List<GameAreaManager>(FindObjectsOfType<GameAreaManager>());
        foreach(GameObject go in areaData.adjacentObjectPrefabs){
            if(!allLoadedAreas.Contains(go.GetComponent<GameAreaManager>())){
                Instantiate(go);
            }
        }
        foreach(GameAreaManager gam in allLoadedAreas){
            if(!areaData.adjacentObjectPrefabs.Contains(gam.gameObject) && gam != this){
                Destroy(gam.gameObject);
            }
        }
    }
}
