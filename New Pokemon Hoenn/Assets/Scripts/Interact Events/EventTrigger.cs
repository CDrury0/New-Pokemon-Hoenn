using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EventTrigger : MonoBehaviour
{
    [SerializeField] private bool destroyIfAlreadyDone;
    [SerializeField] private int _eventTriggerID;
    public int EventTriggerID {
        get { return _eventTriggerID; }
    }
    public InteractEvent interactEvent;
    [SerializeField] private InteractEvent eventIfAlreadyDone;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("InteractPoint")){
            if(eventIfAlreadyDone != null && !GetComponentInParent<GameAreaManager>().completedEvents.Contains(_eventTriggerID)){
                StartCoroutine(interactEvent.DoInteractEvent());
                return;
            }
            StartCoroutine(eventIfAlreadyDone.DoInteractEvent());
        }
    }

    void Awake() {
        if(destroyIfAlreadyDone && GetComponentInParent<GameAreaManager>().completedEvents.Contains(_eventTriggerID)){
            Destroy(gameObject);
        }
    }

    void Reset() {
        _eventTriggerID = FindObjectsOfType<EventTrigger>().Length;
    }
}
