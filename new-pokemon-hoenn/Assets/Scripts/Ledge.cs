using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour, IInterruptPlayerMovement
{
    [SerializeField] private Vector3 _hopDirection;
    public Vector3 HopDirection {get => _hopDirection;}

    public bool Apply(Vector3 direction, out Vector3 movementOffset) {
        var input = PlayerInput.Instance;
        movementOffset = Vector3.zero;
        if(direction != HopDirection)
            return false;

        Collider2D collider = Physics2D.OverlapCircle(
            input.followPoint.position + input.PlayerHeightOffset + (2 * HopDirection),
            0.4f,
            input.movementHandler.InterruptsMovement
        );
        if(collider)
            return false;

        movementOffset = HopDirection * 2;
        return true;
    }
}
