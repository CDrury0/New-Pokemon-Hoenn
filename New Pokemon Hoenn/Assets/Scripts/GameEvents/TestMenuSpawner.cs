using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSpawner : EventAction
{
    public GameObject testMenu;
    protected override IEnumerator EventActionLogic() {
        testMenu.SetActive(!testMenu.activeInHierarchy);
        yield break;
    }
}
