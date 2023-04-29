using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameAreaManager : MonoBehaviour
{
    public AreaData areaData;

    public IEnumerator LoadArea() {
        if(areaData == ReferenceLib.Instance.activeArea){
            yield break;
        }
        Debug.Log("Loading " + areaData.areaName);
        ReferenceLib.Instance.activeArea = areaData;
        List<GameAreaManager> activeGameAreas = new List<GameAreaManager>(FindObjectsOfType<GameAreaManager>());
        List<GameObject> prefabsOfActiveAreas = new List<GameObject>(FindObjectsOfType<GameAreaManager>().Select(a => a.areaData.areaObjectPrefab));
        foreach(GameObject go in prefabsOfActiveAreas){
            if(!areaData.adjacentObjectPrefabs.Contains(go) && go != areaData.areaObjectPrefab){
                Destroy(activeGameAreas.Find(a => a.areaData.areaObjectPrefab == go).gameObject);
            }
        }
        
        foreach(GameObject go in areaData.adjacentObjectPrefabs){
            if(!prefabsOfActiveAreas.Contains(go)){
                Instantiate(go);
            }
        }
    }
}

/* List<GameAreaManager> allLoadedAreas = new List<GameAreaManager>(FindObjectsOfType<GameAreaManager>());
        foreach(GameObject go in areaData.adjacentObjectPrefabs){
            if(!allLoadedAreas.Contains(go.GetComponent<GameAreaManager>())){
                Instantiate(go);
            }
        }
        foreach(GameAreaManager gam in allLoadedAreas){
            if(!areaData.adjacentObjectPrefabs.Contains(gam.gameObject) && gam != this){
                Destroy(gam.gameObject);
            }
        } */
