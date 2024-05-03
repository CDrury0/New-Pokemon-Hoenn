using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    [HideInInspector] public bool Halt;
    [Tooltip("If unset, will be set to player walk speed")] public float moveSpeed;
    [SerializeField] private Animator animator;
    private Vector3 walkToPoint;

    protected abstract IEnumerator PassiveMoveLogic();

    protected IEnumerator MoveLogicWrapper(){
        yield return StartCoroutine(PassiveMoveLogic());
        yield return new WaitUntil(() => PlayerInput.allowMovementInput);
        if(!Halt){
            StartCoroutine(MoveLogicWrapper());
        }
    }

    public IEnumerator WalkStep(Vector3 direction){
        walkToPoint = transform.position + direction;
        AnimateMovement(direction, true);
        yield return new WaitUntil(() => transform.position == walkToPoint);
    }

    public void FacePlayer(Transform movePoint){
        Vector3 toFacePlayer = transform.position - movePoint.position;
        toFacePlayer.Normalize();
        toFacePlayer = GetOppositeDirection(toFacePlayer);
        AnimateMovement(toFacePlayer, false);
    }

    protected static void SetDetectionArea(BoxCollider2D detectionCollider, Vector3 lookDirection, int detectionRange){
        float sizeX = Mathf.Max(Mathf.Abs(detectionRange * lookDirection.x), 0.5f);
        float sizeY = Mathf.Max(Mathf.Abs(detectionRange * lookDirection.y), 0.5f);
        detectionCollider.size = new Vector2(sizeX, sizeY);
        bool offsetAxisIsX = detectionCollider.size.x > detectionCollider.size.y;
        //offsets the collider by half the longest side to compensate the intended distance from the NPC
        detectionCollider.offset = offsetAxisIsX
        ? new Vector2((lookDirection.x == 0 ? 1 : lookDirection.x / Mathf.Abs(lookDirection.x)) * (Mathf.Abs(detectionCollider.size.x * 0.5f) + 0.5f), 0f)
        : new Vector2(0f, (lookDirection.y == 0 ? 1 : lookDirection.y / Mathf.Abs(lookDirection.y)) * (Mathf.Abs(detectionCollider.size.y * 0.5f) + 0.5f));
    }

    protected static Vector3 GetOppositeDirection(Vector3 directionToReflect){
        return directionToReflect * -1;
    }

    protected void AnimateMovement(Vector3 direction, bool isMoving){
        animator.SetBool("IsMoving", isMoving);
        if(isMoving){
            animator.SetBool("WhichStep", !animator.GetBool("WhichStep"));
        }
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    public void Start(){
        if(moveSpeed == 0f){
            moveSpeed = PlayerInput.WALKING_SPEED;
        }
        walkToPoint = transform.position;
        StartCoroutine(MoveLogicWrapper());
    }

    void FixedUpdate(){
        if(walkToPoint != transform.position){
            transform.position = Vector3.MoveTowards(transform.position, walkToPoint, moveSpeed * Time.deltaTime);
        }
    }
}
