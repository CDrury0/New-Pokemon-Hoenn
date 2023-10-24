using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartyInfoBoxButtonContainer : MonoBehaviour
{
    [SerializeField] protected GameObject messageModalPrefab;
    public abstract void LoadActionButtons(Pokemon p);
    public IEnumerator ShowModalMessage(string message){
        GameObject outputInstance = Instantiate(messageModalPrefab);
        IEnumerator writeMessage = outputInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message);
        yield return StartCoroutine(writeMessage);
        Destroy(outputInstance);
    }
}
