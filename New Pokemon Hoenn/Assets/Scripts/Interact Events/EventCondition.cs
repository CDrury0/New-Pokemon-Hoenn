using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCondition : InteractEvent
{
    private enum Operation {Greater, Less, Equal}
    [SerializeField] private EventState state;
    [SerializeField] private Operation operation;
    [SerializeField] private int compareTo;
    [SerializeField] private InteractEvent trueEvent;
    [SerializeField] private InteractEvent falseEvent;

    protected override IEnumerator InteractEventLogic() {
        if(GetCompareSuccess()){
            StartCoroutine(trueEvent.DoInteractEvent());
            yield break;
        }
        StartCoroutine(falseEvent.DoInteractEvent());
    }

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
}
