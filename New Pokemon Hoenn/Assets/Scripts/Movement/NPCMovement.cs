using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    protected Vector3 direction;
    public bool halt;
    [SerializeField] protected MovementAnimation movementAnimation;
    protected abstract IEnumerator MoveLogic();
}
