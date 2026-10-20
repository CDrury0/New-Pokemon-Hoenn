using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMovementMode : EventAction {
    [SerializeField] MovementMode _movementMode;

    protected override IEnumerator EventActionLogic() {
        PlayerInput.Instance.SetMovementMode(_movementMode);
        yield break;
    }
}
