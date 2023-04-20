using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : InteractEvent
{
    public WriteText outputObject;
    public string[] messages;
    public override IEnumerator DoInteractEvent() {
        foreach(string s in messages){
            yield return StartCoroutine(outputObject.WriteMessageConfirm(s));
        }
        outputObject.gameObject.SetActive(false);
    }
}
