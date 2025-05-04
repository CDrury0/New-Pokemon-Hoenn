using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterruptPlayerMovement
{
    public abstract void Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset);
}
