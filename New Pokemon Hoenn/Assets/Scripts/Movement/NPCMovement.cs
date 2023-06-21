using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    [HideInInspector] public bool halt;
    public float moveSpeed = PlayerInput.WALKING_SPEED;
    [SerializeField] protected MovementAnimation movementAnimation;
    private Vector3 walkToPoint;

    protected abstract IEnumerator PassiveMoveLogic();

    protected IEnumerator MoveLogicWrapper(){
        yield return StartCoroutine(PassiveMoveLogic());
        yield return new WaitUntil(() => PlayerInput.allowMovementInput);
        if(!halt){
            StartCoroutine(MoveLogicWrapper());
        }
    }

    public IEnumerator WalkStep(Vector3 direction, float moveSpeed){
        walkToPoint = transform.position + direction;
        StartCoroutine(movementAnimation.AnimateMovement(direction, moveSpeed, true, false));
        yield return new WaitUntil(() => transform.position == walkToPoint);
    }

    public void FacePlayer(Vector3 playerDirection){
        Vector3 toFacePlayer = GetOppositeDirection(playerDirection);
        movementAnimation.FaceDirection(toFacePlayer);
    }

    protected static void SetDetectionArea(BoxCollider2D detectionCollider, Vector3 lookDirection, int detectionRange){
        float sizeX = Mathf.Max(Mathf.Abs(detectionRange * lookDirection.x), 0.5f);
        float sizeY = Mathf.Max(Mathf.Abs(detectionRange * lookDirection.y), 0.5f);
        detectionCollider.size = new Vector2(sizeX, sizeY);
        bool offsetAxisIsX = detectionCollider.size.x > detectionCollider.size.y;
        //offsets the collider by size - 1 to cover the intended distance from the NPC
        detectionCollider.offset = offsetAxisIsX
        ? new Vector2((lookDirection.x == 0 ? 1 : lookDirection.x / Mathf.Abs(lookDirection.x)) * (Mathf.Abs(detectionCollider.size.x) - 1), 0f)
        : new Vector2(0f, (lookDirection.y == 0 ? 1 : lookDirection.y / Mathf.Abs(lookDirection.y)) * (Mathf.Abs(detectionCollider.size.y) - 1));
    }

    protected static Vector3 GetOppositeDirection(Vector3 directionToReflect){
        if(directionToReflect == Vector3.up){
            return Vector3.down;
        }
        else if(directionToReflect == Vector3.down){
            return Vector3.up;
        }
        else if(directionToReflect == Vector3.right){
            return Vector3.left;
        }
        else if(directionToReflect == Vector3.left){
            return Vector3.right;
        }
        else{
            Debug.Log("invalid direction");
            return Vector3.up;
        }
    }

    public void Start(){
        walkToPoint = transform.position;
        StartCoroutine(MoveLogicWrapper());
    }

    void FixedUpdate(){
        if(walkToPoint != transform.position){
            transform.position = Vector3.MoveTowards(transform.position, walkToPoint, moveSpeed * Time.deltaTime);
        }
    }
}
