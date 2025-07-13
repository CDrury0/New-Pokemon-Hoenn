using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventAction : MonoBehaviour
{
    [SerializeField] private bool destroySelfOnComplete;
    [SerializeField] float waitSecondsAfter;
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

    public IEnumerator DoEventAction() {
        yield return StartCoroutine(EventActionLogic());
        if(waitSecondsAfter > 0f)
            yield return new WaitForSeconds(waitSecondsAfter);

        if(!exit && this is not EventSwitch){
            if(nextEvent != null) {
                StartCoroutine(nextEvent.DoEventAction());
            } else {
                PlayerInput.allowMovementInput = true;
                PlayerInput.AllowMenuToggle = true;
            }
        }
        if(destroySelfOnComplete) {
            Destroy(gameObject);
        }
    }

    protected abstract IEnumerator EventActionLogic();
}
