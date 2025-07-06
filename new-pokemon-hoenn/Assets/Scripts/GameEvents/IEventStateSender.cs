using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventStateSender<T>
{
    public List<T> GetState();
}
