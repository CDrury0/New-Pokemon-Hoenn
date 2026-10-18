using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementHandler : ScriptableObject {
    [SerializeField] MovementMode _movementMode;
    public MovementMode MovementMode { get => _movementMode; }

    public static PlayerMovementHandler GetByMovementMode(MovementMode mode) {
        var handlers = Resources.LoadAll<PlayerMovementHandler>("PlayerMovementHandlers");
        foreach(var handler in handlers)
            if(handler.MovementMode == mode)
                return handler;

        return null;
    }

    public abstract void HandleMovement(Vector3 newDirection);

    public abstract void AnimateMovement(Vector3 direction);
}
