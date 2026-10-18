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

    public override void AnimateMovement(PlayerInput input, Vector3 direction)
    {
        
    }

    public override void HandleMovement(PlayerInput input, Vector3 newDirection) {
        input.CurrentMoveSpeed = input.IsSprinting ? SprintSpeed : BaseSpeed;
        if(PlayerInput.Direction != newDirection){
            PlayerInput.Direction = newDirection;
            if(!input.IsSprinting){
                input.DelayMovementInput();
            }
            return;
        }

        var collider = input.GetColliderAtNextStep();
        var interrupt = collider?.GetComponent<IInterruptPlayerMovement>();
        if(collider is not null && interrupt is null)
            return;

        Vector3 moveVector = PlayerInput.Direction;
        var proceed = interrupt?.Apply(input, PlayerInput.Direction, out moveVector) ?? true;
        if(!proceed)
            return;

        PlayerInput.followPoint.position += moveVector;
        AnimateMovement(input, moveVector);

        PlayerInput.StepCount++;

        input.DoAfterStep();
    }
}
