using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EventTrigger : MonoBehaviour
{
    private enum TriggerMethod {Interaction, Position}
    [SerializeField] private bool allowInput;
    [SerializeField] private TriggerMethod triggerMethod;
    [SerializeField] private bool destroyIfAlreadyDone;
    [SerializeField] private EventCondition destroyIfTrue;
    [Tooltip("This ID should be unique within the game area it is a child of")]
    [SerializeField] private int _eventTriggerID;
    public int EventTriggerID {
        get { return _eventTriggerID; }
    }
    [SerializeField] private EventAction eventAction;
    [SerializeField] private EventAction eventIfAlreadyDone;
    [Tooltip("If not null, will stop further supplied NPCMovement MoveLogic from running when this is triggered")]
    [SerializeField] private NPCMovement movementToStop;
    [Tooltip("If not null, will cause the NPC whose NPCMovement component is supplied to face the player when this is triggered")]
    [SerializeField] private NPCMovement movementToFacePlayer;

    void OnTriggerEnter2D(Collider2D collider) {
        if(DoesTriggerMatch(collider)){
            if(movementToStop != null){
                movementToStop.halt = true;
            }
            if(movementToFacePlayer != null){
                movementToFacePlayer.FacePlayer(collider.GetComponentInParent<PlayerInput>().Direction);
            }
            PlayerInput.AllowMenuToggle = allowInput;
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
        eventAction = GetComponent<EventAction>();
    }
}
