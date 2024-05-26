using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStill : NPCMovement
{
    [SerializeField] private Vector2 faceDirection;

    protected override IEnumerator PassiveMoveLogic(){
        // Intentionally empty; still NPCs have no passive movement
        yield break;
    }

    void Awake() {
        SetDetectionArea(faceDirection);
    }
}
