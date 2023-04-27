using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InteractEvent : MonoBehaviour
{
    [SerializeField] protected InteractEvent nextEvent;
    protected abstract IEnumerator InteractEventLogic();
    public IEnumerator DoInteractEvent() {
        yield return StartCoroutine(InteractEventLogic());
        if(nextEvent != null){
            StartCoroutine(nextEvent.DoInteractEvent());
        }
    }
}
