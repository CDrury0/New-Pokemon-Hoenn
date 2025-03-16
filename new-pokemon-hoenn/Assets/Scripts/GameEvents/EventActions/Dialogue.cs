using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : EventAction
{
    public GameObject outputObject;
    public float confirmationDelaySeconds;
    public string[] messages;
    protected override IEnumerator EventActionLogic(EventState state) {
        GameObject activeObject = Instantiate(outputObject);
        WriteText textHandler = activeObject.GetComponentInChildren<WriteText>();
        foreach(string s in messages){
            yield return StartCoroutine(textHandler.WriteMessageConfirm(s, confirmationDelaySeconds));
        }
        Destroy(activeObject);
    }
}
