using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour
{
    [SerializeField] private List<InteractEvent> events;
    protected void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("InteractPoint")){
            StartCoroutine(DoEvents());
        }
    }

    private IEnumerator DoEvents() {
        //block all input during an event?
        for (int i = 0; i < events.Count; i++){
            yield return StartCoroutine(events[i].DoInteractEvent());
        }
    }
}
