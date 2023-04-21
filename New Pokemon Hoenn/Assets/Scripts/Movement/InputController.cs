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
    /// blocks movement of all moveable entities
    /// </summary>
    [SerializeField] public static bool allowMovementInput = true;
    /// <summary>
    /// blocks all input from the player, but not necessarily movement of entities (useful for scripted movement events)
    /// </summary>
    [HideInInspector] public static bool allowMenuToggle = true;

    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }
}
