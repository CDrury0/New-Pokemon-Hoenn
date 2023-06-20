using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    protected Vector3 direction;
    public bool halt;
    [SerializeField] protected MovementAnimation movementAnimation;
    protected abstract IEnumerator MoveLogic();
    protected IEnumerator MoveLogicWrapper(){
        yield return StartCoroutine(MoveLogic());
        yield return new WaitUntil(() => PlayerInput.AllowMenuToggle);
        if(!halt){
            StartCoroutine(MoveLogicWrapper());
        }
    }
    public void FacePlayer(Vector3 playerDirection){
        Vector3 toFacePlayer = GetOppositeDirection(playerDirection);
        StartCoroutine(movementAnimation.AnimateMovement(toFacePlayer, 0, false, false));
    }

    protected Vector3 GetOppositeDirection(Vector3 direction){
        if(direction == Vector3.up){
            return Vector3.down;
        }
        else if(direction == Vector3.down){
            return Vector3.up;
        }
        else if(direction == Vector3.right){
            return Vector3.left;
        }
        else if(direction == Vector3.left){
            return Vector3.right;
        }
        else{
            Debug.Log("invalid direction");
            return Vector3.zero;
        }
    }

    void Start(){
        StartCoroutine(MoveLogicWrapper());
    }
}
