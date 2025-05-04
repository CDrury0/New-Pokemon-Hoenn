using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour, IInterruptPlayerMovement
{
    [SerializeField] private Vector3 _hopDirection;
    public Vector3 HopDirection {get => _hopDirection;}

    public void Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset) {
        movementOffset = Vector3.zero;
        if(direction != HopDirection)
            return;

        Collider2D collider = Physics2D.OverlapCircle(
            PlayerInput.followPoint.position + PlayerInput.PlayerHeightOffset + (2 * HopDirection),
            0.4f,
            input.interruptsMovement
        );
        if(collider)
            return;

        movementOffset = HopDirection * 2;
    }
}
