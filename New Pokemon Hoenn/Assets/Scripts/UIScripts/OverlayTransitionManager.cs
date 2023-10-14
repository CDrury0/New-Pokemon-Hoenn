using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayTransitionManager : MonoBehaviour
{
    public static OverlayTransitionManager Instance { get; private set; }
    [SerializeField] private EventAnimation intro;
    [SerializeField] private EventAnimation outro;

    public void DoTransitionWithAction(UnityEvent action, GameObject toInstantiate){
        StartCoroutine(TransitionCoroutine(action, toInstantiate));
    }

    public void DoTransitionWithAction(System.Action action){
        StartCoroutine(TransitionCoroutine(action));
    }

    //unfortunately this overload is necessary because the Invoke method existing on both types is a coincidence; they have no common base class
    public IEnumerator TransitionCoroutine(UnityEvent action, GameObject toInstantiate = null) {
        yield return StartCoroutine(intro.TransitionLogic());
        if(toInstantiate != null){
            Instantiate(toInstantiate);
        }
        action?.Invoke();
        yield return StartCoroutine(outro.TransitionLogic());
    }

    public IEnumerator TransitionCoroutine(System.Action action) {
        yield return StartCoroutine(intro.TransitionLogic());
        action?.Invoke();
        yield return StartCoroutine(outro.TransitionLogic());
    }

    void Awake() {
        if(Instance != null){
            Debug.Log("OverlayTransitionManager already exists!");
            return;
        }
        Instance = this;
    }
}
