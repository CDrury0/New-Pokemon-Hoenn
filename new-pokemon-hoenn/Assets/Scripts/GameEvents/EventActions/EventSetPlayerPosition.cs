using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetPlayerPosition : EventAction
{
    [SerializeField] private bool lastHealPosition;
    [SerializeField] private bool escapePosition;
    [SerializeField] private Vector3 overridePosition;
    [SerializeField] private Transform overridePositionTransform;
    [SerializeField] private Vector3 forcePlayerDirection;

    protected override IEnumerator EventActionLogic() {
        Vector3 newPosition = GetNewPosition();
        var input = PlayerInput.Instance;
        input.playerTransform.position = newPosition;
        input.followPoint.position = newPosition;
        if(forcePlayerDirection != Vector3.zero){
            input.Direction = forcePlayerDirection;
        }
        yield break;
    }

    private Vector3 GetNewPosition(){
        var input = PlayerInput.Instance;
        Vector3 newPosition = input.playerTransform.position;
        if(lastHealPosition){
            newPosition = ReferenceLib.LastHealPosition.value;
        }
        if(escapePosition){
            newPosition = ReferenceLib.EscapePosition.value;
        }
        if(overridePosition != Vector3.zero){
            newPosition = overridePosition;
        }
        if(overridePositionTransform != null){
            newPosition = overridePositionTransform.position;
        }
        return newPosition;
    }
}
