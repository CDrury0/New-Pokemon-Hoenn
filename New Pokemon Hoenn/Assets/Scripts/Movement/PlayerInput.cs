using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : InputController
{
    public Transform interactPoint;
    public MenuAnimation menuAnimation;
    public LayerMask stopsMovement;
    public SpriteRenderer activePlayerSprite;
    //replace sprite renderer and sprite array with animation controller
    public Sprite[] playerSprites;
    [SerializeField] private float walkingSpeed = 3.5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float movementInputDelaySeconds;
    private Vector3 direction;
    private bool allowMovementInput = true;

    void Update() {
        GetPlayerInput();
    }

    protected void GetPlayerInput() {
        if(Vector3.Distance(transform.position, followPoint.position) == 0){
            if(!GetOtherInput() && allowMovementInput){
                GetMovementInput(Input.GetKey(KeyCode.LeftShift));
            }
        }
    }

    private bool GetOtherInput(){
        if(Input.GetKeyDown(KeyCode.Q)){
            allowMovementInput = !allowMovementInput;
            menuAnimation.ToggleMenu();
            return true;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(ActivateInteractPoint());
            return true;
        }
        return false;
    }

    protected void GetMovementInput(bool sprinting) {
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
        SetPlayerSprite(direction);
        if(!sprinting){
            StartCoroutine(DelayMovementInput());
        }
    }

    protected virtual void SetPlayerSprite(Vector3 directionToFace) {
        if(directionToFace.x != 0){
            if(directionToFace.x == 1){
                activePlayerSprite.sprite = playerSprites[1];
                return;
            }
            activePlayerSprite.sprite = playerSprites[3];
            return;
        }
        if(directionToFace.y == 1){
            activePlayerSprite.sprite = playerSprites[0];
            return;
        }
        activePlayerSprite.sprite = playerSprites[2];
    }

    protected virtual IEnumerator ActivateInteractPoint() {
        //disallow all input until routine has completed?
        interactPoint.position += direction;
        yield return new WaitForFixedUpdate();
        interactPoint.position -= direction;
    }

    private IEnumerator DelayMovementInput() {
        allowMovementInput = false;
        yield return new WaitForSeconds(movementInputDelaySeconds);
        allowMovementInput = true;
    }
}
