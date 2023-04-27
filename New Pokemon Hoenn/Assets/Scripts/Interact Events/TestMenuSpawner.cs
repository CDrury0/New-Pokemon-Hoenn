using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSpawner : InteractEvent
{
    public GameObject testMenu;
    protected override IEnumerator InteractEventLogic() {
        testMenu.SetActive(!testMenu.activeInHierarchy);
        yield break;
    }
}
