using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalkEvent : EventAction
{
    [Tooltip("If true, no x/y coordinates are necessary")]
    [SerializeField] private bool moveToPlayer;
    [Tooltip("By default, Y movement occurs first")]
    [SerializeField] private bool xFirst;
    [SerializeField] private int moveX;
    [SerializeField] private int moveY;
    [SerializeField] private NPCMovement movementComponent;

    protected override IEnumerator EventActionLogic(EventState state){
        int diffY;
        int diffX;
        if(moveToPlayer){
            diffY = (int)((PlayerInput.playerTransform.position.y - transform.position.y));
            diffX = (int)((PlayerInput.playerTransform.position.x - transform.position.x));
            //since the object should be on the tile NEXT TO the player, subtract one from the distance on the final axis traversed
            if(diffY == 0){
                diffX = GetDiffOffset(diffX);
            }
            else if(diffX == 0){
                diffY = GetDiffOffset(diffY);
            }
            else if(xFirst){
                diffY = GetDiffOffset(diffY);
            }
            else{
                diffX = GetDiffOffset(diffX);
            }
        }
        else{
            diffY = moveY;
            diffX = moveX;
        }

        if(xFirst){
            yield return StartCoroutine(WalkDirection(new Vector3(diffX, 0, 0).normalized, diffX));
            yield return StartCoroutine(WalkDirection(new Vector3(0, diffY, 0).normalized, diffY));
            yield break;
        }

        yield return StartCoroutine(WalkDirection(new Vector3(0, diffY, 0).normalized, diffY));
        yield return StartCoroutine(WalkDirection(new Vector3(diffX, 0, 0).normalized, diffX));
    }

    private int GetDiffOffset(int diff){
        return diff / Mathf.Abs(diff) * (Mathf.Abs(diff) - 1);
    }

    private IEnumerator WalkDirection(Vector3 direction, int amount){
        for(int i = 0; i < Mathf.Abs(amount); i++){
            yield return StartCoroutine(movementComponent.WalkStep(direction));
        }
    }

    void Reset(){
        if (GetComponent<NPCMovement>() == null){
            Debug.Log("Remember to add an NPCMovement component!");
        }
    }
}
