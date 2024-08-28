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
        switch(operation){
            case Operation.Greater:
                return state.value > compareTo;
            case Operation.Less:
                return state.value < compareTo;
            default:
                return state.value == compareTo;
        }
    }

    public override bool IsConditionTrue(){
        return GetCompareSuccess();
    }
}
