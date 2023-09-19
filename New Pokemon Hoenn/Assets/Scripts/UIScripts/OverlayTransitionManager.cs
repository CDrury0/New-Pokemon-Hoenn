using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayTransitionManager : MonoBehaviour
{
    public static OverlayTransitionManager Instance { get; private set; }
    [SerializeField] private EventAnimation intro;
    [SerializeField] private EventAnimation outro;

    public void DoTransitionWithAction(UnityEvent action){
        StartCoroutine(TransitionCoroutine(action));
    }

    public IEnumerator TransitionCoroutine(UnityEvent action) {
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
