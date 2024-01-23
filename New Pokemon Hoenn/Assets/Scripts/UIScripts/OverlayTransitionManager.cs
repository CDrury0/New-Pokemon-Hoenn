using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayTransitionManager : MonoBehaviour
{
    public static OverlayTransitionManager Instance { get; private set; }
    [SerializeField] private EventAnimation intro;
    [SerializeField] private EventAnimation outro;

    public void DoTransitionWithAction(UnityEvent action, GameObject toInstantiate, UnityEvent onComplete = null){
        StartCoroutine(TransitionCoroutine(action, toInstantiate, onComplete));
    }

    public void DoTransitionWithAction(System.Action action, System.Action onComplete = null){
        StartCoroutine(TransitionCoroutine(action, onComplete));
    }

    //unfortunately this overload is necessary because the Invoke method existing on both types is a coincidence; they have no common base class
    public IEnumerator TransitionCoroutine(UnityEvent action, GameObject toInstantiate = null, UnityEvent onComplete = null) {
        yield return StartCoroutine(intro.TransitionLogic());
        if(toInstantiate != null){
            Instantiate(toInstantiate);
        }
        action?.Invoke();
        yield return StartCoroutine(outro.TransitionLogic());
        onComplete?.Invoke();
    }

    public IEnumerator TransitionCoroutine(System.Action action, System.Action onComplete = null) {
        yield return StartCoroutine(intro.TransitionLogic());
        action?.Invoke();
        yield return StartCoroutine(outro.TransitionLogic());
        onComplete?.Invoke();
    }

    void Awake() {
        if(Instance != null){
            Debug.Log("OverlayTransitionManager already exists!");
            return;
        }
        Instance = this;
    }
}
