using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameAreaManager : MonoBehaviour
{
    public AreaData areaData;
    [Tooltip("This should self-reference the child object in its hierarchy")]
    public GameObject eventObjectContainer;

    public IEnumerator LoadArea() {
        ReferenceLib.ActiveArea = areaData;

        List<GameAreaManager> activeManagers = new(FindObjectsOfType<GameAreaManager>());
        GameAreaManager currentAreaInstance = activeManagers.Find(area => area.areaData == areaData);
        if(currentAreaInstance == null){
            currentAreaInstance = Instantiate(areaData.areaObjectPrefab).GetComponent<GameAreaManager>();
            activeManagers.Add(currentAreaInstance);
        }
        List<GameObject> prefabsOfActiveAreas = new(activeManagers.Select(a => a.areaData.areaObjectPrefab));

        //destroy all areas that are not in the adjacency list
        foreach(GameObject go in prefabsOfActiveAreas){
            if(go != areaData.areaObjectPrefab && !areaData.adjacentObjectPrefabs.Contains(go)){
                GameObject toDestroy = activeManagers.Find(a => a.areaData.areaObjectPrefab == go).gameObject;
                Destroy(toDestroy);
            }
        }

        //instantiate any area found in the adjacency list that does not already exist in the scene
        foreach(GameObject go in areaData.adjacentObjectPrefabs){
            if(!prefabsOfActiveAreas.Contains(go)){
                Instantiate(go);
            }
        }

        //rebuild current area event object container
        if(areaData.eventObjectPrefab != null){
            Destroy(currentAreaInstance.eventObjectContainer);
            //find the instance to use as parent in case the area loaded was not adjacent to the area the load began from
            currentAreaInstance.eventObjectContainer = Instantiate(areaData.eventObjectPrefab, currentAreaInstance.transform);
        }

        yield break;
    }
}
