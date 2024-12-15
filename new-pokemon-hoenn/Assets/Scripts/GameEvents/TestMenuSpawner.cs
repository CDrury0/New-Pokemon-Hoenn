using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSpawner : EventAction
{
    public GameObject testMenuPrefab;
    private GameObject testMenu;
    public override void DoEventAction(EventState chainState) {
        if(testMenu == null){
            testMenu = Instantiate(testMenuPrefab);
        }
        testMenu.SetActive(!testMenu.activeInHierarchy);
        NextAction(chainState);
    }
}
