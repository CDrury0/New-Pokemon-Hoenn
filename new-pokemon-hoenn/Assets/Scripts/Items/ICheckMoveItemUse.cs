using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckMoveItemUse
{
    public abstract bool CanBeUsed(Pokemon p, int move);
}
