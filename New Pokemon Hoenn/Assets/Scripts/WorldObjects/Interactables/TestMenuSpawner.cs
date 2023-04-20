using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSpawner : InteractEvent
{
    public GameObject testMenu;
    public override IEnumerator DoInteractEvent() {
        testMenu.SetActive(!testMenu.activeInHierarchy);
        yield break;
    }
}
