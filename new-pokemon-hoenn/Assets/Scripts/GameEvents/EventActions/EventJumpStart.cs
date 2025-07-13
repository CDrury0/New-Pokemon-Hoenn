using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventJumpStart : EventAction
{
    void Start() {
        StartCoroutine(DoEventAction());
    }

    protected override IEnumerator EventActionLogic() {
        yield break;
    }
}
