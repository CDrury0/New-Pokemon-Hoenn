using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : InputController
{
    public Transform interactPoint;
    public MenuAnimation menuAnimation;
    public LayerMask stopsMovement;
    public Animator playerAnimator;
    [SerializeField] private float walkingSpeed = 3.5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float movementInputDelaySeconds;
    private Vector3 direction;

    void Update() {
        GetPlayerInput();
    }

    protected void GetPlayerInput() {
        if(Vector3.Distance(transform.position, followPoint.position) == 0){
            if(allowMenuToggle){
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

    protected virtual void GetMovementInput(bool sprinting) {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(horizontal == 0 && vertical == 0){
            return;
        }

        Vector3 newDirection = horizontal != 0 ? new Vector3(horizontal, 0, 0) : newDirection = new Vector3(0, vertical, 0);

        if(direction == newDirection){
            moveSpeed = sprinting ? sprintSpeed : walkingSpeed;
            if(!Physics2D.OverlapCircle(followPoint.position + direction, 0.3f, stopsMovement)){
                followPoint.position += direction;
            }
            return;
        }

        direction = newDirection;

        if(!sprinting){
            StartCoroutine(DelayMovementInput());
        }
    }

    protected virtual IEnumerator ActivateInteractPoint() {
        allowMovementInput = false;
        interactPoint.position += direction;
        yield return new WaitForFixedUpdate();
        interactPoint.position -= direction;
        allowMovementInput = true;
    }

    private IEnumerator DelayMovementInput() {
        allowMovementInput = false;
        yield return new WaitForSeconds(movementInputDelaySeconds);
        allowMovementInput = true;
    }
}
