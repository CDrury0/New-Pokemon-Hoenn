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
    public GameObject interactPointPrefab;
    public Transform followPoint;
    public MenuAnimation menuAnimation;
    public LayerMask stopsMovement;
    public Animator animator;
    private float moveSpeed;
    [SerializeField] private float movementInputDelaySeconds;
    public Vector3 Direction { get; private set; }

    void Update() {
        GetPlayerInput();
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

        Vector3 newDirection = horizontal != 0 ? new Vector3(horizontal, 0, 0) : new Vector3(0, vertical, 0);

        // virtual method that contains the following? (override on surf, bike)
        if(Direction == newDirection){
            moveSpeed = sprinting ? SPRINT_SPEED : WALKING_SPEED;
            if(!Physics2D.OverlapCircle(followPoint.position + Direction, 0.3f, stopsMovement)){
                followPoint.position += Direction;
                AnimateMovement(Direction, true, sprinting);
            }
            return;
        }

        Direction = newDirection;
        //AnimateMovement(Direction, false);

        if(!sprinting){
            StartCoroutine(DelayMovementInput());
        }
        // end virtual method
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

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }

    private IEnumerator ActivateInteractPoint() {
        GameObject pointObj = Instantiate(interactPointPrefab, transform.position + Direction, Quaternion.identity, transform);
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
    }
}
