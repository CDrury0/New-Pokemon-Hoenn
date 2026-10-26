using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementHandlers/PlayerMovementNormal")]
public class PlayerMovementNormal : PlayerMovementHandler {
    [SerializeField] float _baseSpeed;
    public float BaseSpeed { get => _baseSpeed; }
    [SerializeField] float _sprintSpeed;
    public float SprintSpeed { get => _sprintSpeed; }

    public static PlayerMovementNormal GetObject() => Resources.Load<PlayerMovementNormal>("PlayerMovementHandlers/PlayerMovementNormal");

    public override void AnimateMovement(Vector3 direction, bool showMovement=false) {
        var input = PlayerInput.Instance;
        input.animator.SetBool("IsSprinting", input.IsSprinting);
        input.animator.SetBool("IsMoving", showMovement);
        if(showMovement)
            input.animator.SetBool("WhichStep", !input.animator.GetBool("WhichStep"));

        input.animator.SetFloat("X", direction.x);
        input.animator.SetFloat("Y", direction.y);
    }

    public override void HandleMovement(Vector3 newDirection) {
        var input = PlayerInput.Instance;
        input.CurrentMoveSpeed = input.IsSprinting ? SprintSpeed : BaseSpeed;
        if(input.Direction != newDirection){
            input.Direction = newDirection;
            if(!input.IsSprinting){
                input.DelayMovementInput();
            }
            return;
        }

        var collider = input.GetColliderAtNextStep();
        var interrupt = collider?.GetComponent<IInterruptPlayerMovement>();
        if(collider is not null && interrupt is null)
            return;

        Vector3 moveVector = input.Direction;
        var proceed = interrupt?.Apply(input.Direction, out moveVector) ?? true;
        if(!proceed)
            return;

        input.followPoint.position += moveVector;
        AnimateMovement(moveVector, true);

        input.StepCount++;

        input.DoAfterStep();
    }
}
