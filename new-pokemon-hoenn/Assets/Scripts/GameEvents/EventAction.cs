using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventAction : MonoBehaviour
{
    [SerializeField] private bool destroySelfOnComplete;
    [SerializeField] protected EventAction nextEvent;
    /// <summary>
    /// remember to assign the variable under both true and false conditions, 
    /// since it may be reused before another script instance is created
    /// </summary>
    protected bool exit;
    protected abstract IEnumerator EventActionLogic();
    public IEnumerator DoEventAction() {
        yield return StartCoroutine(EventActionLogic());
        if(!exit){
            //kill me
            if(nextEvent != null && this is not EventSwitch){
                StartCoroutine(nextEvent.DoEventAction());
            }
            else{
                PlayerInput.allowMovementInput = true;
                PlayerInput.AllowMenuToggle = true;
            }
        }
        if(destroySelfOnComplete){
            Destroy(gameObject);
        }
    }
}
