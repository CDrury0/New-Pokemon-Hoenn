using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputController : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform followPoint;
    /// <summary>
    /// blocks movement of all moveable entities
    /// </summary>
    [HideInInspector] public static bool allowMovementInput = true;
    /// <summary>
    /// blocks all input from the player, but not necessarily movement of other entities
    /// </summary>
    [HideInInspector] public static bool allowMenuToggle = true;

    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }
}
