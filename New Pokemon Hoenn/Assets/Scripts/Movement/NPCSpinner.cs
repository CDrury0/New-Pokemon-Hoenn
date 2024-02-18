using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpinner : NPCMovement
{
    [SerializeField] private int detectionRange;
    [SerializeField] private BoxCollider2D detectionCollider;
    [SerializeField] private bool rotateClockwise;
    private Vector3[] directionCycle;
    private const float LOOK_DURATION_SECONDS = 1.5f;
    private int index;

    protected override IEnumerator PassiveMoveLogic(){
        AnimateMovement(directionCycle[index], false);
        if (detectionCollider != null){
            SetDetectionArea(detectionCollider, directionCycle[index], detectionRange);
        }
        
        yield return new WaitForSeconds(LOOK_DURATION_SECONDS);
        index = index == directionCycle.Length - 1 ? 0 : index + 1;
    }

    void Awake(){
        directionCycle = rotateClockwise
        ? new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left }
        : new Vector3[] { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
    }
}
