using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EventAction : MonoBehaviour
{
    [SerializeField] protected EventAction nextEvent;
    /// <summary>
    /// remember to assign the variable under both true and false conditions, 
    /// since it may be reused before another script instance is created
    /// </summary>
    protected bool exit;
    protected abstract IEnumerator EventActionLogic();
    public IEnumerator DoEventAction() {
        yield return StartCoroutine(EventActionLogic());
        if(exit){
            //REMOVE THIS ONCE COMBAT SYSTEM EVENT CHAIN IS IMPLEMENTED
            PlayerInput.allowMovementInput = true;
            PlayerInput.AllowMenuToggle = true;
        }
        else if(nextEvent != null){
            StartCoroutine(nextEvent.DoEventAction());
        }
    }
}
