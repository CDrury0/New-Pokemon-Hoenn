using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerMovementHandler : ScriptableObject {
    [SerializeField] MovementMode _movementMode;
    public MovementMode MovementMode { get => _movementMode; }

    public static PlayerMovementHandler GetByMovementMode(MovementMode mode) {
        IEnumerable<PlayerMovementHandler> handlers = Resources.LoadAll<PlayerMovementHandler>("PlayerMovementHandlers");
        return handlers.FirstOrDefault((val) => val.MovementMode == mode);
    }

    public abstract void HandleMovement(Vector3 newDirection);

    public abstract void AnimateMovement(Vector3 direction, bool showMovement=false);
}
