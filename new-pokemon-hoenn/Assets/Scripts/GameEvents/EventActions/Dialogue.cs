using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dialogue : EventStateReceiver<IDialogueReplacer>
{
    public GameObject outputObject;
    public float confirmationDelaySeconds;
    public string[] messages;
    protected override IEnumerator EventActionLogic() {
        GameObject activeObject = Instantiate(outputObject);
        WriteText textHandler = activeObject.GetComponentInChildren<WriteText>();
        foreach(string s in messages){
            string message = s;
            var tables = GetSenderState()?.Select((val) => val.GetReplaceTable());
            if(tables is not null)
                foreach(var table in tables)
                    foreach(var key in table.Keys)
                        message = message.Replace(key, table[key]);

            yield return StartCoroutine(textHandler.WriteMessageConfirm(message, confirmationDelaySeconds));
        }
        Destroy(activeObject);
    }
}
