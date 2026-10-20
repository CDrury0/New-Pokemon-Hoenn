using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementHandlers/PlayerMovementSurf")]
public class PlayerMovementSurf : PlayerMovementHandler {
    [SerializeField] float _baseSpeed;
    public float BaseSpeed { get => _baseSpeed; }

    public static PlayerMovementSurf GetObject() => Resources.Load<PlayerMovementSurf>("PlayerMovementHandlers/PlayerMovementSurf");

    public override void AnimateMovement(Vector3 direction, bool showMovement=false) {
        var input = PlayerInput.Instance;
        input.animator.SetFloat("X", direction.x);
        input.animator.SetFloat("Y", direction.y);
    }

    public override void HandleMovement(Vector3 newDirection) {
        var input = PlayerInput.Instance;
        input.Direction = newDirection;
        var collider = input.GetColliderAtNextStep();
        var interrupt = collider?.GetComponent<IInterruptPlayerMovement>();
        if(collider is not null && interrupt is null)
            return;

        Vector3 moveVector = input.Direction;
        var proceed = interrupt?.Apply(input.Direction, out moveVector) ?? true;
        if(!proceed)
            return;

        input.followPoint.position += moveVector;
        AnimateMovement(moveVector);

        input.StepCount++;

        input.DoAfterStep();
    }
}
