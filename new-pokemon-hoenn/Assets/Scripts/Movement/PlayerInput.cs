using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static Transform playerTransform;
    public const float WALKING_SPEED = 3.5f;
    public const float SPRINT_SPEED = 6.5f;
    /// <summary>
    /// This may be set independently to account for menu toggling
    /// </summary>
    public static bool allowMovementInput = true;
    private static bool _allowMenuToggle = true;
    /// <summary>
    /// Setting this false automatically sets allowMovementInput to false
    /// </summary>
    public static bool AllowMenuToggle {
        get { return _allowMenuToggle; }
        set { if(value == false){
                allowMovementInput = false;
            }
            _allowMenuToggle = value;
        }
    }
    public static int StepCount;
    public static event Action<int> StepEvent;
    public GameObject interactPointPrefab;
    public static Transform followPoint;
    public MenuAnimation menuAnimation;
    public LayerMask stopsMovement;
    public Animator animator;
    private float moveSpeed;
    [SerializeField] private float movementInputDelaySeconds;
    public static Vector3 Direction { get; set; }
    public static Vector3 PlayerHeightOffset => new(0, -0.5f, 0);

    void Update() {
        GetPlayerInput();
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }

    private void GetPlayerInput() {
        if (Vector3.Distance(transform.position, followPoint.position) != 0){
            return;
        }
        if (!AllowMenuToggle){
            return;
        }
        if (GetMenuInput() || !allowMovementInput) {
            return;
        }
        if (GetInteractInput()) {
            return;
        }
        GetMovementInput(Input.GetKey(KeyCode.LeftShift));            
    }
            

    private bool GetInteractInput() {
        if(Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(ActivateInteractPoint());
            return true;
        }
        return false;
    }

    private bool GetMenuInput() {
        if(Input.GetKeyDown(KeyCode.Q)){
            allowMovementInput = !allowMovementInput;
            menuAnimation.ToggleMenu();
            return true;
        }
        return false;
    }

    private void GetMovementInput(bool sprinting) {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(horizontal == 0 && vertical == 0){
            AnimateMovement(Direction, false);
            return;
        }

        Vector3 newDirection = horizontal != 0 ? new(horizontal, 0, 0) : new(0, vertical, 0);
        moveSpeed = sprinting ? SPRINT_SPEED : WALKING_SPEED;

        // virtual method that contains the following? (override on surf, bike)
        if(Direction != newDirection){
            Direction = newDirection;
            if(!sprinting){
                StartCoroutine(DelayMovementInput());
            }
            return;
        }

        Collider2D obstacleCollider = Physics2D.OverlapCircle(followPoint.position + PlayerHeightOffset + Direction, 0.4f, stopsMovement);
        if(obstacleCollider){
            IModifyPlayerMovement movementModifier = obstacleCollider.gameObject.GetComponent<IModifyPlayerMovement>();
            if(movementModifier == null){
                return;
            }
            movementModifier.Apply(this, newDirection);
        } else {
            followPoint.position += Direction;
        }

        AnimateMovement(Direction, true, sprinting);

        StepCount++;
        StartCoroutine(StepEventRoutine());
        // end virtual method
    }

    IEnumerator StepEventRoutine() {
        yield return new WaitForFixedUpdate();
        StepEvent?.Invoke(StepCount);
    }

    private void AnimateMovement(Vector3 direction, bool isMoving, bool isSprinting = false) {
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsMoving", isMoving);
        if(isMoving){
            animator.SetBool("WhichStep", !animator.GetBool("WhichStep"));
        }
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    private IEnumerator ActivateInteractPoint() {
        GameObject pointObj = Instantiate(interactPointPrefab, transform.position + PlayerHeightOffset + Direction, Quaternion.identity, transform);
        yield return new WaitForFixedUpdate();
        Destroy(pointObj);
    }

    private IEnumerator DelayMovementInput() {
        AnimateMovement(Direction, false);
        allowMovementInput = false;
        yield return new WaitForSeconds(movementInputDelaySeconds);
        allowMovementInput = true;
    }

    void Awake(){
        playerTransform = transform;
        followPoint = GameObject.FindGameObjectWithTag("MovePoint").transform;
    }

    void Start(){
        if(SaveManager.LoadedSave?.currentPosition != null){
            transform.position = SaveManager.LoadedSave.currentPosition.value;
        }
        followPoint.position = transform.position;
    }
}
