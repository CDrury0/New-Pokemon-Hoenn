using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkEvent : EventAction {
    [Tooltip("If empty, moves 1 unit in the current facing direction")]
    [SerializeField] Vector3 moveBy;
    protected override IEnumerator EventActionLogic() {
        Vector3 prevPosition = PlayerInput.Instance.playerTransform.position;
        PlayerInput.Instance.followPoint.position += moveBy.magnitude == 0f ? PlayerInput.Instance.Direction : moveBy;
        yield return new WaitUntil(() => Vector3.Distance(prevPosition + moveBy, PlayerInput.Instance.playerTransform.position) == 0f);
    }
}
