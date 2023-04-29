using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : EventAction
{
    [SerializeField] private GameAreaManager areaEntered;

    protected override IEnumerator EventActionLogic() {
        StartCoroutine(areaEntered.LoadArea());
        yield break;
    }
}
