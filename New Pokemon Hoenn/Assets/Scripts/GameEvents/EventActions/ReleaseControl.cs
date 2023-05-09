using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseControl : EventAction
{
    protected override IEnumerator EventActionLogic(){
        PlayerInput.AllowMenuToggle = true;
        PlayerInput.allowMovementInput = true;
        yield break;
    }
}
