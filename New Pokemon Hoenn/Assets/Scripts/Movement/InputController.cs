using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputController : MonoBehaviour
{
    public const float PLAYER_WALK_SPEED = 3.5f;
    [SerializeField] protected float moveSpeed = PLAYER_WALK_SPEED;
    [SerializeField] protected Transform followPoint;
    /// <summary>
    /// when false, blocks all moveable objects from moving independently (but they can still be moved via events)
    /// </summary>
    public static bool allowMovementInput = true;
    /// <summary>
    /// when false, blocks the player from toggling the menu
    /// </summary>
    public static bool allowMenuToggle = true;

    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }
}
