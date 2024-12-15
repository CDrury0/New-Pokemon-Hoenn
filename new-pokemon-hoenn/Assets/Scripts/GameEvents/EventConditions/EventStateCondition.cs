using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EventStateCondition : EventCondition
{
    private enum Operation {Greater, Less, Equal}
    [SerializeField] private EventState state;
    [SerializeField] private Operation operation;
    [SerializeField] private int compareTo;

    public override bool IsConditionTrue(){
        return IsConditionTrue(state);
    }

    public override bool IsConditionTrue(EventState localState) {
        return operation switch {
            Operation.Greater => localState.value > compareTo,
            Operation.Less => localState.value < compareTo,
            Operation.Equal => localState.value == compareTo,
            _ => throw new InvalidEnumArgumentException("Invalid operation enum state"),
        };
    }
}
