using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputController : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform followPoint;

    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }
}
