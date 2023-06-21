using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalkEvent : EventAction
{
    [Tooltip("If true, no x/y coordinates are necessary")]
    [SerializeField] private bool moveToPlayer;
    [SerializeField] private int moveX;
    [SerializeField] private int moveY;
    [SerializeField] private MovementAnimation movementAnimation;
    [SerializeField] private NPCMovement movementComponent;
    private Vector3 moveToPoint;

    protected override IEnumerator EventActionLogic(){
        //REMOVE ONCE ALERT EVENT ACTION IS IMPLEMENTED
        yield return new WaitForSeconds(1f);

        int diffY;
        int diffX;
        if(moveToPlayer){
            diffY = (int)((PlayerInput.playerTransform.position.y - transform.position.y));
            diffX = (int)((PlayerInput.playerTransform.position.x - transform.position.x));
            //since the object should be on the tile NEXT TO the player, subtract one from the distance on the final axis traversed
            if(diffY == 0){
                diffX = (diffX / Mathf.Abs(diffX)) * (Mathf.Abs(diffX) - 1);
            }
            else if(diffX == 0){
                diffY = (diffY / Mathf.Abs(diffY)) * (Mathf.Abs(diffY) - 1);
            }
            else{
                diffX = (diffX / Mathf.Abs(diffX)) * (Mathf.Abs(diffX) - 1);
            }
        }
        else{
            diffY = moveY;
            diffX = moveX;
        }

        Vector3 directionToMoveY = new Vector3(0, diffY > 0 ? 1 : -1, 0);
        for (int i = 0; i < Mathf.Abs(diffY); i++){
            yield return StartCoroutine(movementComponent.WalkStep(directionToMoveY, movementComponent.moveSpeed));
        }
        Vector3 directionToMoveX = new Vector3(diffX > 0 ? 1 : -1, 0, 0);
        for (int i = 0; i < Mathf.Abs(diffX); i++){
            yield return StartCoroutine(movementComponent.WalkStep(directionToMoveX, movementComponent.moveSpeed));
        }
    }

    void Reset(){
        Debug.Log("Remember to add an NPCMovement component!");
    }
}
