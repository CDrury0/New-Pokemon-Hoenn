using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStateCondition : EventCondition
{
    private enum Operation {Greater, Less, Equal}
    [SerializeField] private EventState state;
    [SerializeField] private Operation operation;
    [SerializeField] private int compareTo;

    private bool GetCompareSuccess() {
        return operation switch {
            Operation.Greater => state.value > compareTo,
            Operation.Less => state.value < compareTo,
            _ => state.value == compareTo,
        };
    }

    public override bool IsConditionTrue(){
        return GetCompareSuccess();
    }
}
