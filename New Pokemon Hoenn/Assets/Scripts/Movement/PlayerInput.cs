using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const float WALKING_SPEED = 3.5f;
    private const float SPRINT_SPEED = 7.5f;
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
        set {if(value == false){
                allowMovementInput = false;
            }
            _allowMenuToggle = value;
        }
    }
    public Transform followPoint;
    public Transform interactPoint;
    public MenuAnimation menuAnimation;
    public LayerMask stopsMovement;
    public MovementAnimation playerAnimations;
    private float moveSpeed;
    [SerializeField] private float movementInputDelaySeconds;
    public Vector3 Direction { get; private set; }

    void Update() {
        GetPlayerInput();
    }

    private void GetPlayerInput() {
        if(Vector3.Distance(transform.position, followPoint.position) == 0){
            if(AllowMenuToggle){
                if(!GetMenuInput() && allowMovementInput){
                    if(!GetInteractInput()){
                        GetMovementInput(Input.GetKey(KeyCode.LeftShift));
                    }
                }
            }
        }
    }

    private bool GetInteractInput() {
        if(Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(ActivateInteractPoint());
            return true;
        }
        return false;
    }

    private bool GetMenuInput(){
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
            return;
        }

        Vector3 newDirection = horizontal != 0 ? new Vector3(horizontal, 0, 0) : newDirection = new Vector3(0, vertical, 0);

        if(Direction == newDirection){
            moveSpeed = sprinting ? SPRINT_SPEED : WALKING_SPEED;
            if(!Physics2D.OverlapCircle(followPoint.position + Direction, 0.3f, stopsMovement)){
                followPoint.position += Direction;
                StartCoroutine(playerAnimations.AnimateMovement(Direction, moveSpeed, true, sprinting));
            }
            return;
        }

        Direction = newDirection;

        if(!sprinting){
            StartCoroutine(DelayMovementInput());
        }
    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, moveSpeed * Time.deltaTime);
    }

    private IEnumerator ActivateInteractPoint() {
        allowMovementInput = false;
        interactPoint.position += Direction;
        interactPoint.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        interactPoint.gameObject.SetActive(false);
        interactPoint.position -= Direction;
        allowMovementInput = true;
    }

    private IEnumerator DelayMovementInput() {
        allowMovementInput = false;
        yield return new WaitForSeconds(movementInputDelaySeconds);
        allowMovementInput = true;
    }
}
