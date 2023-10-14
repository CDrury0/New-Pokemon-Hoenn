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

    public IEnumerator TransitionCoroutine(UnityEvent action, GameObject toInstantiate) {
        yield return StartCoroutine(intro.TransitionLogic());
        if(toInstantiate != null){
            Instantiate(toInstantiate);
        }
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
