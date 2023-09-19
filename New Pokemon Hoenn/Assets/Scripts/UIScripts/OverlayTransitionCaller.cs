using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayTransitionCaller : MonoBehaviour
{
    public UnityEvent action;

    /// <summary>
    /// This method does not wait for the transition. Use the coroutine if you meant to wait.
    /// </summary>
    public void CallTransition() {
        OverlayTransitionManager.Instance.DoTransitionWithAction(action);
    }

    public IEnumerator CallTransitionCoroutine() {
        yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(action));
    }
}
