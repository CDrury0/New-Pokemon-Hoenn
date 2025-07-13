using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : EventAction, IEventStateSender
{
    [SerializeField] private float timeToWait;
    [Tooltip("The IEventStateSender to pass to the instanced transitionObjPrefab")]
    [SerializeField] EventAction stateToPass;
    [Tooltip("The gameobject with attached animation that should be played when this event action activates")]
    [SerializeField] private GameObject transitionObjPrefab;
    [SerializeField] private bool destroyOwnTransitionAfterWait;
    [Tooltip("If not null, destroys the provided transition object immediately after this animation begins; this is useful when waiting for other actions to finish before another animation segment occurs.")]
    public EventAnimation previousTransitionToDestroy;
    private GameObject instantiatedTransitionObj;

    public List<T> SendState<T>() where T : class {
        return (stateToPass as IEventStateSender)?.SendState<T>();
    }

    protected override IEnumerator EventActionLogic() {
        yield return StartCoroutine(TransitionLogic());
    }

    /// <summary>
    /// For use outside the scope of an EventAction chain
    /// </summary>
    public IEnumerator TransitionLogic() {
        instantiatedTransitionObj = Instantiate(transitionObjPrefab);
        if(stateToPass is not null){
            var receiver = instantiatedTransitionObj.GetComponentInChildren<IStateReceiverUpdatable>();
            receiver?.SetStateSender(this);
            StartCoroutine((receiver as EventAction)?.DoEventAction());
        }
        previousTransitionToDestroy?.DestroyInstantiatedTransition();
        yield return new WaitForSeconds(timeToWait);
        if(destroyOwnTransitionAfterWait)
            DestroyInstantiatedTransition();
    }

    protected void DestroyInstantiatedTransition() {
        Destroy(instantiatedTransitionObj);
        instantiatedTransitionObj = null;
    }
}
