using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifyPlayerMovement
{
    public abstract void Apply(PlayerInput input, Vector3 direction);
}
