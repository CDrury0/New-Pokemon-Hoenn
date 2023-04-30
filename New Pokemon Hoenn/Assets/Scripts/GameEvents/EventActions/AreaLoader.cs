using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is necessary because the GameAreaManager will likely not be instantiated, meaning a coroutine cannot be started from it
/// </summary>
public class AreaLoader : EventAction
{
    [SerializeField] private GameAreaManager areaEntered;

    protected override IEnumerator EventActionLogic() {
        StartCoroutine(areaEntered.LoadArea());
        yield break;
    }
}
