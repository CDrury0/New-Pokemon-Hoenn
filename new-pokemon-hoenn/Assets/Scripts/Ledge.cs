using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour, IModifyPlayerMovement
{
    [SerializeField] private Vector3 _hopDirection;
    public Vector3 HopDirection {get => _hopDirection;}

    public void Apply(PlayerInput input, Vector3 direction) {
        if(direction != HopDirection)
            return;

        Collider2D collider = Physics2D.OverlapCircle(
            PlayerInput.followPoint.position + PlayerInput.PlayerHeightOffset + (2 * HopDirection),
            0.4f,
            input.stopsMovement
        );
        if(collider)
            return;

        PlayerInput.followPoint.position = PlayerInput.playerTransform.position + (HopDirection * 2);
    }
}
