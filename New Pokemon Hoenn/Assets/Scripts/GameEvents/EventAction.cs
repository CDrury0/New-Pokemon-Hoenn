using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EventAction : MonoBehaviour
{
    [SerializeField] protected EventAction nextEvent;
    protected abstract IEnumerator EventActionLogic();
    public IEnumerator DoEventAction() {
        yield return StartCoroutine(EventActionLogic());
        if(nextEvent != null){
            StartCoroutine(nextEvent.DoEventAction());
        }
    }
}
