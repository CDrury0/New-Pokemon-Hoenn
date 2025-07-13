using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateReceiverUpdatable
{
    public void SetStateSender(IEventStateSender sender);
}
