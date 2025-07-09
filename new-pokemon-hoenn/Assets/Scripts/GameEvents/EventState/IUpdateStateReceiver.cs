using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateStateReceiver
{
    public void SetStateSender(IEventStateSender sender);
}
