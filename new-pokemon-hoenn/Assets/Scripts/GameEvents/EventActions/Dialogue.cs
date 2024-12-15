using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : EventAction
{
    public GameObject outputObject;
    public float confirmationDelaySeconds;
    public string[] messages;
    public override void DoEventAction(EventState chainState) {
        StartCoroutine(ChainGang(DialogueCoroutine(), () => NextAction(chainState)));
    }

    protected IEnumerator DialogueCoroutine() {
        GameObject activeObject = Instantiate(outputObject);
        WriteText textHandler = activeObject.GetComponentInChildren<WriteText>();
        foreach(string s in messages){
            yield return StartCoroutine(textHandler.WriteMessageConfirm(s, confirmationDelaySeconds));
        }
        Destroy(activeObject);
    }
}
