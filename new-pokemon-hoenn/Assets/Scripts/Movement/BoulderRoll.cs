using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRoll : MonoBehaviour, IInterruptPlayerMovement {
    [SerializeField] FieldMoveCondition requiredMove;

    public bool Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset) {
        movementOffset = Vector3.zero;
        if(!requiredMove?.IsConditionTrue() ?? false)
            return false;

        if(Physics2D.OverlapCircle(transform.position + direction, 0.4f, input.interruptsMovement))
            return false;

        transform.position += direction;
        StartCoroutine(input.DelayMovementInput());
        input.AnimateMovement(direction, true);
        return false;
    }
}
