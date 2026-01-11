using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour, IInterruptPlayerMovement
{
    [SerializeField] private Vector3 _hopDirection;
    public Vector3 HopDirection {get => _hopDirection;}

    public bool Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset) {
        movementOffset = Vector3.zero;
        if(direction != HopDirection)
            return false;

        Collider2D collider = Physics2D.OverlapCircle(
            PlayerInput.followPoint.position + PlayerInput.PlayerHeightOffset + (2 * HopDirection),
            0.4f,
            input.interruptsMovement
        );
        if(collider)
            return false;

        movementOffset = HopDirection * 2;
        return true;
    }
}
