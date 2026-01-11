using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterruptPlayerMovement
{
    /// <returns>Whether or not to actually take a step after this function returns</returns>
    public abstract bool Apply(PlayerInput input, Vector3 direction, out Vector3 movementOffset);
}
