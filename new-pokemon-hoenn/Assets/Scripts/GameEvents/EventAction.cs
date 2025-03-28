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
    protected bool _proceed;
    /// <summary>
    /// For pausing event action logic until a trigger is invoked elsewhere
    /// </summary>
    protected bool Proceed {
        get {
            if(_proceed){
                _proceed = false;
                return true;
            }
            return false;
        }
        set {
            _proceed = value;
        }
    }

    protected abstract IEnumerator EventActionLogic(EventState state);
    public IEnumerator DoEventAction(EventState state) {
        yield return StartCoroutine(EventActionLogic(state));
        if(!exit){
            //kill me
            if(nextEvent != null && this is not EventSwitch){
                StartCoroutine(nextEvent.DoEventAction(state));
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
