using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dialogue : EventStateReceiver<IStringReplacer>
{
    public GameObject outputObject;
    public float confirmationDelaySeconds;
    public string[] messages;
    protected override IEnumerator EventActionLogic(EventState state) {
        GameObject activeObject = Instantiate(outputObject);
        WriteText textHandler = activeObject.GetComponentInChildren<WriteText>();
        foreach(string s in messages){
            string message = s;
            var stateSender = GetStateSender();
            if(stateSender == null)
                continue;

            var tables = stateSender.GetState().Select((val) => val.GetReplaceTable());
            foreach(var table in tables){
                foreach(var key in table.Keys){
                    message = s.Replace(key, table[key]);
                }
            }

            yield return StartCoroutine(textHandler.WriteMessageConfirm(message, confirmationDelaySeconds));
        }
        Destroy(activeObject);
    }
}
