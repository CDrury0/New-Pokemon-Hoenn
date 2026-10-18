using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRoll : MonoBehaviour, IInterruptPlayerMovement {
    [SerializeField] FieldMoveCondition requiredMove;

    public bool Apply(Vector3 direction, out Vector3 movementOffset) {
        var input = PlayerInput.Instance;
        movementOffset = Vector3.zero;
        if(!requiredMove?.IsConditionTrue() ?? false)
            return false;

        if(Physics2D.OverlapCircle(transform.position + direction, 0.4f, input.interruptsMovement))
            return false;

        transform.position += direction;
        input.DelayMovementInput();
        input.movementHandler.AnimateMovement(direction);
        return false;
    }
}
