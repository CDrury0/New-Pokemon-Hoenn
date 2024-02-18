using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    [HideInInspector] public bool Halt {
        get { return animator.GetBool("Halt"); }
        set { animator.SetBool("Halt", value); }
    }
    [Tooltip("If unset, will be set to player walk speed")] public float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] protected MovementAnimation movementAnimation;
    private Vector3 walkToPoint;

    protected abstract IEnumerator PassiveMoveLogic();

    protected IEnumerator MoveLogicWrapper(){
        yield return StartCoroutine(PassiveMoveLogic());
        yield return new WaitUntil(() => PlayerInput.allowMovementInput);
        if(!Halt){
            StartCoroutine(MoveLogicWrapper());
        }
    }

    public IEnumerator WalkStep(Vector3 direction, float moveSpeed){
        walkToPoint = transform.position + direction;
        StartCoroutine(movementAnimation.AnimateMovement(direction, moveSpeed, true, false));
        yield return new WaitUntil(() => transform.position == walkToPoint);
    }

    public void FacePlayer(Transform movePoint){
        Vector3 toFacePlayer = transform.position - movePoint.position;
        toFacePlayer.Normalize();
        toFacePlayer = GetOppositeDirection(toFacePlayer);
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
        return directionToReflect * -1;
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
