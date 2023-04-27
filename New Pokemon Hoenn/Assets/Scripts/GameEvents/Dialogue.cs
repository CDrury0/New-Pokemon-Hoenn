using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : EventAction
{
    public WriteText outputObject;
    public string[] messages;
    protected override IEnumerator EventActionLogic() {
        foreach(string s in messages){
            yield return StartCoroutine(outputObject.WriteMessageConfirm(s));
        }
        outputObject.gameObject.SetActive(false);
    }

    void Reset() {
        outputObject = GameObject.FindGameObjectWithTag("NarrativeText").GetComponent<WriteText>();
    }
}
