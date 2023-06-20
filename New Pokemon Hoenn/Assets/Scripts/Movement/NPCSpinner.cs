using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpinner : NPCMovement
{
    [SerializeField] private bool rotateClockwise;
    private Vector3[] directionCycle;
    private const float LOOK_DURATION_SECONDS = 1.5f;
    private int index;
    protected override IEnumerator MoveLogic(){
        yield return StartCoroutine(movementAnimation.AnimateMovement(directionCycle[index], 0, false, false));
        //the coroutine won't actually return any time since it is only changing the sprite once, so manual waiting is necessary
        yield return new WaitForSeconds(LOOK_DURATION_SECONDS);
        index = index == directionCycle.Length - 1 ? 0 : index + 1;
    }

    void Awake(){
        directionCycle = rotateClockwise
        ? new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left }
        : new Vector3[] { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
    }
}
