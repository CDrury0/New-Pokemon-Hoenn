using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSpawner : EventAction
{
    public GameObject testMenuPrefab;
    private GameObject testMenu;
    protected override IEnumerator EventActionLogic() {
        if(testMenu == null){
            testMenu = Instantiate(testMenuPrefab);
        }
        testMenu.SetActive(!testMenu.activeInHierarchy);
        yield break;
    }
}
