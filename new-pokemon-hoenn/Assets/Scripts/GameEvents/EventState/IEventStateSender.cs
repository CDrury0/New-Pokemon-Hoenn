using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventStateSender
{
    public List<T> GetState<T>() where T : class;
}
