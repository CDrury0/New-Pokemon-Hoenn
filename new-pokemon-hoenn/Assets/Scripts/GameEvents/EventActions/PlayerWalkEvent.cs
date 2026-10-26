using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkEvent : EventAction {
    [Tooltip("If empty, moves 1 unit in the current facing direction")]
    [SerializeField] Vector3 moveBy;
    protected override IEnumerator EventActionLogic() {
        var input = PlayerInput.Instance;
        input.PlayerTakeStepForced(moveBy.magnitude == 0f ? input.Direction : moveBy);
        yield return new WaitUntil(() => !input.PlayerIsMoving());
    }
}
