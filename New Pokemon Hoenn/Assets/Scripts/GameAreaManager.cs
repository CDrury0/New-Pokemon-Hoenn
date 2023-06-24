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
        ReferenceLib.Instance.activeArea = areaData;
        List<GameAreaManager> activeGameAreas = new List<GameAreaManager>(FindObjectsOfType<GameAreaManager>());
        List<GameObject> prefabsOfActiveAreas = new List<GameObject>(FindObjectsOfType<GameAreaManager>().Select(a => a.areaData.areaObjectPrefab));

        //destroy all areas that are not in the adjacency list
        foreach(GameObject go in prefabsOfActiveAreas){
            if(!areaData.adjacentObjectPrefabs.Contains(go)){
                GameObject toDestroy = activeGameAreas.Find(a => a.areaData.areaObjectPrefab == go).gameObject;
                Destroy(toDestroy);
            }
        }

        if(areaData.eventObjectPrefab != null){
            Destroy(eventObjectContainer);
            eventObjectContainer = Instantiate(areaData.eventObjectPrefab, transform);
        }

        //instantiate any area found in the adjacency list that does not already exist in the scene
        foreach(GameObject go in areaData.adjacentObjectPrefabs){
            if(!prefabsOfActiveAreas.Contains(go)){
                Instantiate(go);
            }
        }

        yield break;
    }
}
