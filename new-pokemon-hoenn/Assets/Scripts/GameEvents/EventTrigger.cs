using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class EventTrigger : MonoBehaviour
{
    private enum TriggerMethod {Interaction, Position}
    [SerializeField] private bool allowInput;
    [SerializeField] private TriggerMethod triggerMethod;
    [SerializeField] private bool destroyIfAlreadyDone;
    [SerializeField] private EventCondition destroyIfTrue;
    [Tooltip("Set this field to value -1 to auto-generate a value unique to its Event Container. Min auto-value: 0. Auto-generating also gives all Event Triggers that are children of this one a new ID.")]
    [SerializeField] private int _eventTriggerID;
    public int EventTriggerID {
        get { return _eventTriggerID; }
    }
    [SerializeField] private EventAction[] eventActions;
    [SerializeField] private EventAction[] eventsIfAlreadyDone;
    [Tooltip("If not null, will stop further supplied NPCMovement MoveLogic from running when this is triggered even after event chain completes")]
    [SerializeField] private NPCMovement movementToStop;
    [Tooltip("If not null, will cause the NPC whose NPCMovement component is supplied to face the player when this is triggered (useful on trainer interact)")]
    [SerializeField] private NPCMovement movementToFacePlayer;
    private bool movePointEligible;
    private Transform movePointTransform;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("MovePoint")){
            movePointEligible = true;
            movePointTransform = collider.transform;
            return;
        }
        if (!DoesTriggerMatch(collider)) {
            return;
        }

        bool markedDone = GetComponentInParent<GameAreaManager>().areaData.eventManifest.Contains(_eventTriggerID);
        if(markedDone && eventsIfAlreadyDone.Length == 0){
            return;
        }

        //otherwise, SOME EventAction will occur
        if(movementToStop != null){
            movementToStop.Halt = true;
        }
        if(movementToFacePlayer != null){
            movementToFacePlayer.FacePlayer(movePointTransform);
        }
        PlayerInput.AllowMenuToggle = allowInput;
        if(eventsIfAlreadyDone.Length == 0 || !markedDone){
            foreach (EventAction e in eventActions){
                StartCoroutine(e.DoEventAction());
            }
            return;
        }
        foreach(EventAction e in eventsIfAlreadyDone){
            StartCoroutine(e.DoEventAction());
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.CompareTag("MovePoint")){
            movePointEligible = false;
        }
    }

    private bool DoesTriggerMatch(Collider2D collider) {
        if(triggerMethod == TriggerMethod.Interaction && collider.CompareTag("InteractPoint")){
            return true;
        }
        else if((movePointEligible || collider.transform.position == movePointTransform.position) && triggerMethod == TriggerMethod.Position && collider.CompareTag("Player")){
            return true;
        }
        return false;
    }

    void Awake() {
        movePointTransform = GameObject.FindGameObjectWithTag("MovePoint").transform;
        if((destroyIfAlreadyDone && GetComponentInParent<GameAreaManager>().areaData.eventManifest.Contains(_eventTriggerID)) || (destroyIfTrue != null && destroyIfTrue.IsConditionTrue())){
            Destroy(gameObject);
        }
    }

    void OnValidate() {
        if (_eventTriggerID == -1){
            AssignUniqueID();
        }
    }

    private void AssignUniqueID(){
        GameObject eventContainer = GetParentWithTag(gameObject, "EventContainer");
        List<EventTrigger> allTriggers = eventContainer.GetComponentsInChildren<EventTrigger>().ToList();
        int maxID = 0;
        foreach(EventTrigger e in allTriggers){
            maxID = Mathf.Max(maxID, e.EventTriggerID);
        }
        _eventTriggerID = maxID + 1;

        // If there is a child GameObject with an EventTrigger component, assign that a new unique ID as well (recursive)
        EventTrigger childTrigger = GetComponentsInChildren<EventTrigger>().ToList().Find(e => e != this);
        childTrigger?.AssignUniqueID();
    }

    public static GameObject GetParentWithTag(GameObject child, string tag){
        if(child == null){
            return null;
        } 
        else if(child.CompareTag(tag)){
            return child;
        }
        return GetParentWithTag(child.transform.parent.gameObject, tag);
    }
}
