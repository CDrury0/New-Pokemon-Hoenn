using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInputPermission : EventAction
{
    public enum Assignment {TRUE, FALSE, DEFAULT}
    [SerializeField] private Assignment allowMenuToggle;
    [SerializeField] private Assignment allowMovementInput;
    protected override IEnumerator EventActionLogic(){
        if(allowMenuToggle != Assignment.DEFAULT){
            InputController.allowMenuToggle = (int)allowMenuToggle == 0;
        }
        if(allowMovementInput != Assignment.DEFAULT){
            InputController.allowMovementInput = (int)allowMovementInput == 0;
        }
        yield break;
    }
}
