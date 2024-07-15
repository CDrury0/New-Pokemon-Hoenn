using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameAreaManager : MonoBehaviour
{
    public static GameAreaManager CurrentAreaInstance;
    public AreaData areaData;
    [Tooltip("This should self-reference the child object in its hierarchy")]
    public GameObject eventObjectContainer;

    public IEnumerator LoadArea() {
        LoadArea(areaData);
        yield break;
    }

    public static void LoadArea(AreaData toLoad){
        ReferenceLib.ActiveArea = toLoad;

        List<GameAreaManager> activeManagers = new(FindObjectsOfType<GameAreaManager>());
        GameAreaManager currentAreaInstance = activeManagers.Find(area => area.areaData == toLoad);
        if(currentAreaInstance == null){
            currentAreaInstance = Instantiate(toLoad.areaObjectPrefab).GetComponent<GameAreaManager>();
            activeManagers.Add(currentAreaInstance);
        }
        List<GameObject> prefabsOfActiveAreas = new(activeManagers.Select(a => a.areaData.areaObjectPrefab));

        //destroy all areas that are not in the adjacency list
        foreach(GameObject go in prefabsOfActiveAreas){
            if(go != toLoad.areaObjectPrefab && !toLoad.adjacentObjectPrefabs.Contains(go)){
                GameObject toDestroy = activeManagers.Find(a => a.areaData.areaObjectPrefab == go).gameObject;
                Destroy(toDestroy);
            }
        }

        //instantiate any area found in the adjacency list that does not already exist in the scene
        foreach(GameObject go in toLoad.adjacentObjectPrefabs){
            if(!prefabsOfActiveAreas.Contains(go)){
                Instantiate(go);
            }
        }

        //rebuild current area event object container
        if(toLoad.eventObjectPrefab != null){
            Destroy(currentAreaInstance.eventObjectContainer);
            //find the instance to use as parent in case the area loaded was not adjacent to the area the load began from
            currentAreaInstance.eventObjectContainer = Instantiate(toLoad.eventObjectPrefab, currentAreaInstance.transform);
        }

        CurrentAreaInstance = currentAreaInstance;
    }
}
