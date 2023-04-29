using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : EventAction
{
    public GameObject outputObject;
    public string[] messages;
    protected override IEnumerator EventActionLogic() {
        GameObject activeObject = Instantiate(outputObject);
        WriteText textHandler = activeObject.GetComponentInChildren<WriteText>();
        foreach(string s in messages){
            yield return StartCoroutine(textHandler.WriteMessageConfirm(s));
        }
        Destroy(activeObject);
    }
}
