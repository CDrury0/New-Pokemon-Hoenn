using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EventTrigger : MonoBehaviour
{
    private enum TriggerMethod {Interaction, Position}
    [SerializeField] private TriggerMethod triggerMethod;
    [SerializeField] private bool destroyIfAlreadyDone;
    [SerializeField] private EventCondition destroyIfTrue;
    [SerializeField] private int _eventTriggerID;
    public int EventTriggerID {
        get { return _eventTriggerID; }
    }
    [SerializeField] private EventAction eventAction;
    [SerializeField] private EventAction eventIfAlreadyDone;

    void OnTriggerEnter2D(Collider2D collider) {
        if(DoesTriggerMatch(collider)){
            if(eventIfAlreadyDone == null || !GetComponentInParent<GameAreaManager>().areaData.eventManifest.Contains(_eventTriggerID)){
                StartCoroutine(eventAction.DoEventAction());
                return;
            }
            StartCoroutine(eventIfAlreadyDone.DoEventAction());
        }
    }

    private bool DoesTriggerMatch(Collider2D collider) {
        if(triggerMethod == TriggerMethod.Interaction && collider.CompareTag("InteractPoint")){
            return true;
        }
        else if(triggerMethod == TriggerMethod.Position && collider.CompareTag("Player")){
            return true;
        }
        return false;
    }

    void Awake() {
        if((destroyIfAlreadyDone && GetComponentInParent<GameAreaManager>().areaData.eventManifest.Contains(_eventTriggerID)) || (destroyIfTrue != null && destroyIfTrue.IsConditionTrue())){
            Destroy(gameObject);
        }
    }

    void Reset() {
        _eventTriggerID = GetComponentInParent<GameAreaManager>().GetComponentsInChildren<EventTrigger>().Length;
    }
}
